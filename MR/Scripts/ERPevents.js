
/*
This deletes a users data from the database by passing the users id to the needed action
*/  function UserDelete(/*/AJAX/CIMGet/1532*/ actionLink,
/*must assign, used for calling javaScript methods*/ formFields) {
    if (window.confirm("Confirm to delete record?")) {

        var token = $('input[name = "__RequestVerificationToken"]').val();

        var aLink = actionLink.split('/');
        actionLink = "/" + aLink[1] + "/" + aLink[2];
        aLink = aLink[3];

        $.ajax({
            type: "POST",
            url: actionLink,
            data: { __RequestVerificationToken: token, Id: aLink },
            success: function (response) {
                formFields.reset();
                alert(response);
                scrollToTop();
            },
            failure: function (response) { alert('FAILURE>>>> ' + response.responseText); },
            error: function (response) { alert('ERROR>>>> ' + response.responseText); }
        });
    }
};

/*
On click btnLookUpID
   open modal(remove djq-hide from modalID, add djq-vo to body)

var new & former;
listen for keyPress
   if searchVal != ''
   {
    new is set
    (if one digit & former == undefined)
        { load new sorted table
        set formerValueToNew }
    (if one digit & newInput == former)
        { show all hidden }
    (if one digit & newInput != former)
        { remove table contents
        load new sorted table
        set formerValueToNew }
    (if .length >1 and current != former)
        { get current searchTerm and hide wrongRows }
   }

On click btnClose
      close(add djq-hide to modal, remove djq-vo from body)
*/  function LoadDynamicDT2(
/*Pass the ID of the lookUp Button*/ btnLookUpID,
/*Pass the ID of the modal that displays this lookup e.g #refLookUp2*/ modalId,
/*Pass the ID of the OriginaL table e.g #refTable2*/ tableID,
/*Pass the ID of the text-box to be filled on modal close e.g #txtReference*/ dataReturnID,
/*Pass the index of the columnInTheTable whose data is to be returned to the txtBox*/ colIDToReturn,
/*Pass the colNo that needs to be worked on*/ searchColNo, boolStrictSearch) {

    var nStr, fStr, searchTerm;
    var myModal = $(modalId);
    var iii = 0;
    var formerST = '';
    var prSt = 0;

    $('body ' + btnLookUpID).on({
        click: function () {
            $(dataReturnID).removeProp('disabled', 'disabled');
            $('body').addClass('djq-vo');
            myModal.removeClass('djq-hide');

            nStr, searchTerm, fStr = undefined;
            myModal.find('#mySearchTerm').val('');

            myModal.find('#mySearchTerm').focus();
            if (iii != 0) {
                myModal.find('.djq-body12 table tbody .djq-hide ,'
                            + ' .djq-body2 table tbody .djq-hide').removeClass('djq-hide');
            }
            iii++;
            CreateDynamicDT2(myModal, tableID, dataReturnID, colIDToReturn);

            formerST = '';
        }
    });

    myModal.find('#mySearchTerm').keyup(function () {
        searchTerm = myModal.find('#mySearchTerm').val().trim().toLowerCase();

        var min = 2;
        var max = 3;

        if (searchTerm.length == max && formerST == '') {
            SearchDynamicDT2(myModal, tableID, tableID, searchTerm, searchColNo);
            formerST = searchTerm;
        }
        else if (searchTerm.length == max && formerST == searchTerm) {
            SearchDynamicDT2(myModal, tableID, tableID, searchTerm, searchColNo);
        }
        else if (searchTerm.length == max && formerST != searchTerm) {
            myModal.find(".djq-body12 table tbody .djq-hide , "
               + ".djq-body2 table tbody .djq-hide ").removeClass("djq-hide");
            SearchDynamicDT2(myModal, tableID, tableID, searchTerm, searchColNo);
            formerST = searchTerm;
        }
        else if (searchTerm.length > max) {
            if (prSt == 0) {
                prSt = searchTerm.length;
                SearchDynamicDT2(myModal, tableID, tableID, searchTerm, searchColNo);
            }
            else if (searchTerm.length < prSt) {
                prSt = searchTerm.length;
                SearchDynamicDT2(myModal, tableID, tableID, searchTerm, searchColNo, 1);
            }
            else {
                prSt = searchTerm.length;
                SearchDynamicDT2(myModal, tableID, tableID, searchTerm, searchColNo);
            }
        }
        else if (searchTerm.length < max) {
            myModal.find(".djq-body12 table tbody .djq-hide , "
               + ".djq-body2 table tbody .djq-hide").removeClass("djq-hide");
        }
    });

    myModal.find('.close-dataJQ').on({
        click: function () {
            myModal.addClass('djq-hide');
            $('body').removeClass('djq-vo');
        }
    });

};

/*
Get the table contents from the DB
Generate the html for the two tables(the first helps to show a static header,
    the second shows the scrollable contents)
Remove the two old tables and paste the new one
Attach listening events to the click, dblClick and select actions
*/  function CreateDynamicDT2(
/*Pass the modalDOM from load e.g myModal*/ myModal,
/*Pass the ID of the OriginaL table e.g #refTable2*/ tableID,
/*Pass the ID of the text-box to be filled on modal close e.g #txtReference*/ dataReturnID,
/*Pass the index of the column whose data is to be returned to the txtBox E.G. 1 or 2 not 0*/ colIDToReturn, boolStrictSearch) {

    var table1 = '.djq-body12 > ' + tableID;
    var table2 = '.djq-body2 > ' + tableID;
    var btnSelect = myModal.find(' .select-dataJQ');
    var cssClassName = 'dataTableRST';
    //RST = RowSelectToggle also applies a color overlay to indicate the currently selected class

    //The following handles click, dblClick and the return of value to the user
    myModal.find('.djq-body2 > ' + tableID).off('click', 'tr'); //To remove events misbehaviour
    myModal.find(' .select-dataJQ').off('click'); //To remove events misbehaviour

    btnSelect.prop('disabled', 'disabled');

    myModal.find(table2).on('click', 'tr', function () {

        if ($(this).hasClass(cssClassName)) {
            $(this).removeClass(cssClassName);
            myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
            btnSelect.prop('disabled', 'disabled');
        }
        else {
            myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
            $(this).addClass(cssClassName);
            btnSelect.removeProp('disabled');
        }
    });

    var dataReturn;
    var dRID = $(dataReturnID);
    myModal.find(table2).on('dblclick', 'tr', function () {
        dataReturn = $(this).find('td:nth-child(' + colIDToReturn + ')').text();

        myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
        btnSelect.prop('disabled', 'disabled');

        myModal.addClass('djq-hide');
        $('body').removeClass('djq-vo');
        dRID.val(dataReturn);
        dRID.focus();
    });

    btnSelect.on('click', function () {
        dataReturn = myModal.find(table2 + ' .' + cssClassName + ' td:nth-child(' + colIDToReturn + ')').text();

        myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
        btnSelect.prop('disabled', 'disabled');

        myModal.addClass('djq-hide');
        $('body').removeClass('djq-vo');
        dRID.val(dataReturn);
        dRID.focus();
    });
};

/*
get the text
search it
if(it does not start with the string)
{hide}
*/  function SearchDynamicDT2(
/*Pass the modalDOM from load e.g myModal */ myModal,
/*Pass the ID of the first table e.g #refTable2*/ tableID1,
/*Pass the ID of the second table e.g #refTable2*/ tableID2, searchTerm,
/*Pass the colNo that needs to be worked on*/ searchColNo, backSrch) {

    var modalDOM = myModal;

    var i = 1;
    if (backSrch != 1) {
        modalDOM.find(".djq-body12 table tbody tr").each(function () {

            if (modalDOM
                .find(".djq-body12 table tbody tr:nth-child(" + i + ") td:nth-child(" + searchColNo + ")")
                .html().toLowerCase()
                .search(searchTerm) == -1) {

                modalDOM
                    .find(".djq-body12 table tbody tr:nth-child(" + i + "), "
                    + ".djq-body2 table tbody tr:nth-child(" + i + ")")
                    .addClass("djq-hide");
            }
            i++;
        });
    }
    else {
        modalDOM.find(".djq-body12 table tbody .djq-hide").each(function () {

            if (modalDOM
                .find(".djq-body12 table tbody tr:nth-child(" + i + ") td:nth-child(" + searchColNo + ")")
                .html().toLowerCase()
                .search(searchTerm) != -1) {

                modalDOM
                    .find(".djq-body12 table tbody tr:nth-child(" + i + "), "
                    + ".djq-body2 table tbody tr:nth-child(" + i + ")")
                    .removeClass("djq-hide");
            }
            i++;
        });
    }

};

/*
This function does not have the delete button
This function interacts with the original table that has the ID that was passed, removes such a table from the DOM
and then replaces the table with current data. It automatically activates the click and double click events for the table.
This function also accepts a compulsory 'startChar' value which determines the subset of rows that will be displayed to
the user. This therefore serves as a solution to scenarios whereby the retrieval of all the data available in a table
will be inefficient in solving a task.
*/  function activateJsonDT2(controllerName, actionName,
/*Pass the data-table header names as an array*/ dataTableHeaderNames,
/*Pass the database column names as an array*/ dbColNames,
/*Pass the starting character of rowData to return THIS VALUE MUST BE PASSED*/ startChar,
/*Pass the index of the column whose data is to be returned to the txtBox*/ colIDToReturn,
/*Pass the ID of the text-box to be filled on modal close e.g #txtReference*/ dataReturnID,
/*Pass the ID of the modal that displays this lookup e.g #refLookUp2*/ modalId,
/*Pass the ID of the parent of the data-table e.g #refTable2Parent*/ optionalTableIdParent,
/*Pass the ID of the OriginaL data-table e.g #refTable2*/ optionalTableId) {

    modalId = modalId.slice(1);
    optionalTableIdParent = optionalTableIdParent.slice(1);
    optionalTableId = optionalTableId.slice(1);

    $('#' + optionalTableId).remove();

    var token = $('input[name = "__RequestVerificationToken"]').val();
    var aLink = startChar;

    $.ajax({
        type: "POST",
        url: '/' + controllerName + '/' + actionName,
        data: { __RequestVerificationToken: token, Id: aLink },
        success: function (data, status, xhr) {

            var items = '<table id="' + optionalTableId + '" class="table table-striped table-bordered data-table" ' +
                    'style="width:100%;"> <thead> <tr>';

            for (var arrayIndex in dataTableHeaderNames) {
                items += '<th>' + dataTableHeaderNames[arrayIndex] + '</th>';
            };

            items += '</tr> </thead> <tbody>';

            $.each(data, function (i, hrPersonnel) {
                items += "<tr>";

                for (var arrayIndex in dbColNames) {
                    items += "<td>" + hrPersonnel[dbColNames[arrayIndex]] + "</td>";
                };

                items += '</tr>';
            });

            items += "</tbody></table>";

            $('#' + optionalTableIdParent).html(items).children('table#' + optionalTableId).DataTable();

            LoadStaticDT(colIDToReturn, dataReturnID, '#' + modalId, '#' + optionalTableId, true);
        },
        error: function (xhr, status, error) { alert('NETWORK ERROR\n\nCheck your connection and try again'); }
    });
};

/*This function activates the delete button thats present in the ID of the dataTable that's being passed
*/  function DeleteDTRow(/*Pass the ID of the data-table that has .btnDelete class
on its delete button e.g #refTable1 */ dataTableId) {

    var ta = $(dataTableId).DataTable();
    ta.on("click", ".btnDelete", function () {
        ta.row('tr' + '#' + $(this).attr('id')).remove();
        ta.draw(false);
        /*
        This type of delete button uses the id thats placed at the button
        and the same id taht's placed at the parent row of the button
        eg
        <tr id='@@Model.id'>
            <td></td>
            <td></td>
            <td> <button id='@@Model.id'>Delete</button> </td>
        </tr>
        */
    });
};

function AutoFillForm(dOM, ctrlDtas, ctrlNum, ref, FillUpTable) {

    var rett = 0;
    var ftbl = 0;

    ctrlDtas = ctrlDtas.split('~~~~');
    ctrlDtas = ctrlDtas[ctrlNum].split('++++');

    if (ctrlNum != '' && ref == ctrlDtas[1]) {

        rett = 1;
        var aa = ctrlDtas.length;

        if (FillUpTable == 1) {
            ftbl = 1;
            dOM.find('#FillUpTable1' + ' tbody').html(ctrlDtas[aa - 1]);
        }

        for (var i = 0; i < aa - ftbl; i++) {

            var dis = dOM.find('[name="' + ctrlDtas[i] + '"]');
            var curVal = ctrlDtas[++i];

            if (dis.hasClass("eTxt")) {
                dis.val(curVal);
            }
            else if (dis.hasClass("eChk")) {
                dis.prop('checked', curVal);
            }
            else if (dis.hasClass("eNum")) {
                dis.val(curVal.toFixed(2));
            }
            else if (dis.hasClass("eDte")) {
                dis.val(formatDate2(curVal));
            }
        }
    }

    return rett;
};

/*
This requests for a string from the server
and fills the appropriate textBox with the string
*/  function FillTBox(/* /AJAX/CIMGet/ */actionLink,
/* dOM Variable */dOM,
/* control or textbox Id with the hash tag e.g #txtReference*/ controlId) {

    var token = $('input[name = "__RequestVerificationToken"]').val();

    var aLink = actionLink.split('/');
    actionLink = "/" + aLink[1] + "/" + aLink[2];
    aLink = aLink[3];

    $.ajax({
        type: "POST",
        url: actionLink,
        data: { __RequestVerificationToken: token, Id: aLink },
        success: function (data, status, xhr) {
            dOM.find(controlId).val(data);
        },
        error: function (xhr, status, error) {
            //alert('ERROR: This page will reload shortly.>>>> ' + xhr.xhrText); location.reload(true);
        }
    });

};

/*
This function gets data from the server
*/  function GetData(/*Pass the link to the url '/controllerName/actionName'
*/ actionLink, sDta) {

    if (sDta === undefined) { sDta = {}; }

    var rett = '';
    $.ajax({
        type: "POST",
        url: actionLink,
        data: sDta,
        success: function (data, status, xhr) { rett = data; },
        async: false,
        error: function (xhr, status, error) { alert('NETWORK ERROR\n\nCheck your connection and try again'); }
    });
    return rett;
};

/*
This function gets data from the server and returns an object
*/  function GetData2(/*Pass the link to the url '/controllerName/actionName'
*/ actionLink, sDta, callback) {
    openFch();
    if (sDta === undefined) { sDta = {}; }

    var rett = {};
    $.ajax({
        type: "POST",
        url: actionLink,
        data: sDta,
        success: function (data, status, xhr) {
            rett = data;
            if (typeof callback == "function") { closeFch(); callback(rett); }
        },
        error: function (xhr, status, error) {
            if (error == '') { closeFch(); alert('NETWORK ERROR\n\nCheck your connection and try again'); }
        }
    });
};

/*This function activates the click and double click event for the data-table in the modal-ID being passed
*/  function LoadStaticDT(/*Pass the index of the column whose data is to be returned to the txtBox*/ colIDToReturn,
/*This parameter must be passed e.g #txtReference*/ dataReturnID,
/*Pass the ID of the modal that displays this lookup e.g #refLookUp1*/ modalId,
/*Pass the ID of the Data-Table e.g #refTable1*/ optionalTableId) {

    $(optionalTableId).DataTable();
    if (optionalTableId === undefined) { optionalTableId = '.data-table'; }
    $(modalId + ' .select-dataJQ').prop('disabled', 'disabled');

    var cssClassName = 'dataTableRST'; //RST means= RowSelectToggle

    $(modalId + ' ' + optionalTableId + ' tbody').on('click', 'tr', function () {

        if ($(this).hasClass(cssClassName)) {
            $(this).removeClass(cssClassName);
            $(modalId + ' .select-dataJQ').prop('disabled', 'disabled');
        }
        else {
            $(modalId + ' ' + optionalTableId).DataTable().$('tr.' + cssClassName).removeClass(cssClassName);
            $(this).addClass(cssClassName);
            $(modalId + ' .select-dataJQ').removeProp('disabled');
        }
    });

    var dataReturn;
    $(modalId + ' .data-table tbody').on('dblclick', 'tr', function () {
        dataReturn = $(modalId + ' ' + optionalTableId).DataTable().row(this).data();

        $('*').removeClass(cssClassName);
        $(modalId + ' .select-dataJQ').prop('disabled', 'disabled');

        dataReturn = dataReturn[colIDToReturn];
        var dRID = $(dataReturnID);
        dRID.val(dataReturn);
        $(modalId + '.modalJQ').modal('hide');
        dRID.focus();
    });

    $(modalId + ' .select-dataJQ').on('click', function () {
        dataReturn = $(modalId + ' ' + optionalTableId).DataTable().row('.' + cssClassName).data();

        $('*').removeClass(cssClassName);
        $(modalId + ' .select-dataJQ').prop('disabled', 'disabled');

        dataReturn = dataReturn[colIDToReturn];
        var dRID = $(dataReturnID);
        dRID.val(dataReturn);
        $(modalId + '.modalJQ').modal('hide');
        dRID.focus();
    });

};

/*
    An hidden field must be available inside the table with the id='tableData'
        The above hidden field is what gets submitted to the server on click of btnSubmit
    The id of the table is passed.
    NOTE: controls whose data are to be added to the table must have a unique class 
    e.g. .add2Table1, .add2Table2, or .taxDta1, .taxDta2 etc.
        This helps in getting the correct data irrespective of the arrangements of controls
    An optional parameter is also added to indicate(i.e. 'True') whether to add the delete button or not
*/  function AddTableRow(/* pass the dOM Variable */ dOM,
/* pass the ID of the table e.g #customerItems */ tableId,
/* pass the number of columns available in the table
   this is used to repetitively get the value in a control e.g 5 */ colNo,
/* pass the ID of a unique div which encloses all control values thats to be
   added to the table. This also uniquely identifies data when there are multiple instances
   where this method needs to be called e.g #customerItems */ uniqueDIV,
/* pass the optional parameter for the delete button  i.e. '1' for True and 0 for false */
addDeleteButton, callBCK) {

    //attach new col header if delete is true
    /*
    if (addDeleteButton == 1 && !dOM.find(tableId + ' thead tr th:last i').hasClass('fa-remove')) {
        var a = dOM.find(tableId + ' thead tr').html();
        dOM.find(tableId + ' thead tr').html(a + '<th class="alc"><i class="fa fa-remove"></i></th>');
    }
    */
    //call the contents of tbody and attach
    var serialNo = 1;

    dOM.find(tableId + ' tbody tr').each(function () {
        serialNo++;
    });
    var b = '<tr><td class="all">' + serialNo + '</td>';
    if (addDeleteButton == 1)
    { b = '<tr><td class="all"><span class="serialNo">' + serialNo + '</span> <a href="#" class="fa fa-trash deleteRow"></a></td>'; }

    for (var i = 1; i <= colNo; i++) {
        if (dOM.find(uniqueDIV + i).val() == '') {
            b += '<td class="all">' + 'null' + '</td>';
        }
        else {
            b += '<td class="all">' + dOM.find(uniqueDIV + i).val() + '</td>';
        }
    }
    //if (addDeleteButton == 1) { b += '<td class="alc"><a href="#" class="fa fa-trash deleteRow"></a></td>'; }
    b += '</tr>';

    dOM.find(tableId + ' tbody').html(dOM.find(tableId + ' tbody').html()+b);
    //dOM.find(tableId + ' tbody').append(b);

    //listen for clicks on the trash button
    if (addDeleteButton == 1) {

        dOM.find(tableId + ' .deleteRow').on({
            click: function () {

                if (dOM.find(this).hasClass('noNo')) { return false; }
                else if (window.confirm("CONFIRM!!!\n\nClick 'Yes' to Delete Record...")) {
                    dOM.find(this).parent().parent().remove();

                    var j = 1;
                    dOM.find(tableId + ' tbody tr').each(function () {
                        $(this).find('td:first .serialNo').text(j + " ");
                        j++;
                    });


                    var zz = ''; var bb = "~"; var cc = 1;
                    dOM.find(tableId + ' tbody tr').each(function () {
                        if (cc > 1) { zz += bb; }
                        for (var i = 1; i <= colNo; i++) {
                            if (i < colNo) {
                                zz += $(this).find('td:nth-child(' + (i + 1) + ')').text() + '^';
                            }
                            else { zz += $(this).find('td:nth-child(' + (i + 1) + ')').text(); }
                        }
                        cc++;
                    });
                    dOM.find(tableId + ' #tableData').val(zz);
                }
            }
        });
    }

    var zz = ''; var j = 1;
    dOM.find(tableId + ' tbody tr').each(function () {
        if (j > 1) { zz += "~"; }
        for (var i = 1; i <= colNo; i++) {
            if (i < colNo) {
                zz += $(this).find('td:nth-child(' + (i + 1) + ')').text() + '^';
            }
            else { zz += $(this).find('td:nth-child(' + (i + 1) + ')').text(); }
        }
        j++;
    });
    dOM.find(tableId + ' #tableData').val(zz);

    if (typeof callBCK == "function") { callBCK(zz); }
};

/*
   The id of the table is passed and all the contents of the tbody is then emptied.
*/  function DeleteAllRows(/* pass the dOM Variable */ dOM,
/* pass the ID of the Clear List button e.g #clearList */ btnClearList,
    /* pass the ID of the table e.g #customerItems */ tableId) {

    dOM.find(btnClearList).on({
        click: function () {
            dOM.find(tableId + ' tbody').empty();
        }
    });
};

/*
    If need be, call this method again for another table
*/
function clearFillUpT(dOM, /* pass the table ID e.g #table1 */ tableID) {
    dOM.find(tableID + " tbody").html('');
};














/*
On click btnLookUpID
   open modal(remove djq-hide from modalID, add djq-vo to body)

var new & former;
listen for keyPress
   if searchVal != ''
   {
    new is set
    (if one digit & former == undefined)
        { load new sorted table
        set formerValueToNew }
    (if one digit & newInput == former)
        { show all hidden }
    (if one digit & newInput != former)
        { remove table contents
        load new sorted table
        set formerValueToNew }
    (if .length >1 and current != former)
        { get current searchTerm and hide wrongRows }
   }

On click btnClose
      close(add djq-hide to modal, remove djq-vo from body)
*/  function LoadDynamicDT1(
/*Pass the c# server side generated ajax-URL*/ jaxURL,
/*Pass the data-table header names as an array*/ tableHeadArray,
/*Pass the database column names as an array*/ dbColNamesArray,
/*Pass the database table name e.g VCTRANS */ dbParentName,
/*Pass the ID of the lookUp Button*/ btnLookUpID,
/*Pass the ID of the modal that displays this lookup e.g #refLookUp2*/ modalId,
/*Pass the ID of the OriginaL table e.g #refTable2*/ tableID,
/*Pass the ID of the text-box to be filled on modal close e.g #txtReference*/ dataReturnID,
/*Pass the index of the columnInTheTable whose data is to be returned to the txtBox*/ colIDToReturn,
/*Pass the ID of an optional field you want to use on the server*/
opsPara, callback) {
    var nStr, fStr, searchTerm = '';
    var myModal = $(modalId);
    var prSt = 0;

    $('body ' + btnLookUpID).on({
        click: function () {

            myModal.find('.search-dataJQ').click();

            $(dataReturnID).removeProp('disabled', 'disabled');
            $('body').addClass('djq-vo');
            myModal.removeClass('djq-hide');

            nStr, searchTerm, fStr = undefined;
            myModal.find('#mySearchTerm').val('');

            myModal.find('#mySearchTerm').focus();
            myModal.find('.djq-body2 table').html('');

            myModal.find("#djq-aval").addClass('hide');
            myModal.find("#djq-result").html('');
            myModal.find('#first , #next , #previous , #last').prop('disabled', true);
        }
    });

    myModal.find('#mySearchTerm').keyup(function () {

        var ssrch = myModal.find('#mySearchTerm').val().trim();

        if (ssrch == '' || ssrch.length >= 3) {
            myModal.find('#btnSearch').click();
        }
    });

    myModal.find('#btnSearch').on({
        click: function () {
            var ssrch = myModal.find('#mySearchTerm').val().trim();
            var searchTerm = ssrch;
            var max = 0;

            if (searchTerm == '') { searchTerm = ' '; }
            else { searchTerm.toLowerCase(); max = 3; }


            if (ssrch != '' && ssrch.length < max) {
                alert('ERROR\n\n PLEASE ENTER A MINIMUM OF THREE(3) CHARACTERS');
                myModal.find('#mySearchTerm').focus();
            }
            else {

                myModal.find('#first , #next , #previous , #last').prop('disabled', true);
                myModal.find('.djq-body2 table').css("opacity", "0"); //++++++++++++++++++++++++++++++++++++++++++++++++++
                myModal.find('.djq-body2 table, #djq-result, ' +
                    '#first , #next , #previous , #last').addClass('hide');

                var stLgth = searchTerm.length;

                if (stLgth >= max) {
                    CreateDynamicDT1(jaxURL, searchTerm + '~1', tableHeadArray, dbColNamesArray, dbParentName,
                        myModal, tableID, dataReturnID, colIDToReturn, opsPara, callback);
                }
                else if (stLgth < max) {
                    myModal.find(".djq-body2 table tbody , #djq-result ").html('');
                }
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                myModal.find('.djq-body2 table, #djq-result, ' +
                '#first , #next , #previous , #last').removeClass('hide');
                myModal.find('.djq-body2 table').animate({ "opacity": "1" }, 1500, 'swing'); //++++++++++++++++++++++++++++++++++++++++++++

                if (!myModal.find("#djq-aval").hasClass('aval')) {
                    myModal.find("#djq-aval").removeClass('hide');
                }
            }
        }
    });

    myModal.find('.close-dataJQ').on({
        click: function () {
            myModal.addClass('djq-hide');
            $('body').removeClass('djq-vo');
        }
    });

};

/*
Get the table contents from the DB
Generate the html for the two tables(the first helps to show a static header,
    the second shows the scrollable contents)
Remove the two old tables and paste the new one
Attach listening events to the click, dblClick and select actions
*/  function CreateDynamicDT1(actionLink/*controllerName*/, searchTerm,
/*Pass the data-table header names as an array*/ tableHeadArray,
/*Pass the database column names as an array*/ dbColNamesArray,
/*Pass the database table name e.g VCTRANS */ dbParentName,
/*Pass the modalDOM from load e.g myModal*/ myModal,
/*Pass the ID of the OriginaL table e.g #refTable2*/ tableID,
/*Pass the ID of the text-box to be filled on modal close e.g #txtReference
 a unique class name can also be passed which will be used to identify the controls
 that need to be filled E.G .cData
*/ dataReturnID,
/*Pass the index of the column whose data is to be returned to the txtBox E.G. 1 or 2 not 0
 an array can also be passed eg[ 1,2,3] if you want to fill multiple controls with data
*/ colIDToReturn,
opsPara, callback) {

    openFch();
    var table2 = '.djq-body2 > ' + tableID;
    var btnSelect = myModal.find(' .select-dataJQ');
    var cssClassName = 'dataTableRST';
    //RST = RowSelectToggle also applies a color overlay to indicate the currently selected class

    myModal.find("#djq-aval").removeClass('aval');
    myModal.find("#djq-aval").addClass('hide');

    var token = $('input[name = "__RequestVerificationToken"]').val();

    var aLink = searchTerm
    var apnd = $(opsPara).val();
    if (apnd != null && apnd != undefined && apnd != '' && apnd.trim() != '') {
        aLink += '~' + ($(opsPara).hasClass('isCHK') ? ($(opsPara).prop('checked') ? apnd : "False") : apnd);
    }

    $.ajax({
        type: "POST",
        url: actionLink,
        data: { __RequestVerificationToken: token, Id: aLink },
        success: function (data, status, xhr) {

            closeFch();
            var items = '';

            //if (btnPage != 1)
            //{
            items = '<thead> <tr><th>S/N</th>';
            for (var arrayIndex in tableHeadArray) {
                items += '<th>' + tableHeadArray[arrayIndex] + '</th>';
            };
            items += '</tr></thead><tbody>';
            //}

            var kk = 0;
            $.each(data[dbParentName], function (i, hrPersonnel) {
                items += "<tr><td>" + (++kk) + "</td>";

                for (var arrayIndex in dbColNamesArray) {
                    var aa = dbColNamesArray[arrayIndex];
                    if (aa.toLowerCase().search('date') == -1) {
                        items += "<td>" + hrPersonnel[dbColNamesArray[arrayIndex]] + "</td>";
                    }

                    else {
                        items += "<td>" + formatDate1(hrPersonnel[dbColNamesArray[arrayIndex]]) + "</td>";
                    }

                };

                items += '</tr>';
            });

            //if (btnPage != 1) {
            items += "</tbody>"; myModal.find(' .djq-body2 table').html(items);
            //}
            //else{ myModal.find(' .djq-body2 table tbody').html(items); }

            var firstLast = data['SYSCODETABSvm']['ERPmiscl']['FillUpTable'];
            firstLast = firstLast.split("++++");

            var first = firstLast[0];
            var last = firstLast[1];

            if (kk == 0) { myModal.find("#djq-result").html('Zero results found'); }
            else {
                myModal.find('#djq-result').html('Page ' + first + ' of ' + last);
            }
            myModal.find('#djq-curVal').html(first);
            myModal.find('#djq-max').html(last);

            if (first == 1 && last == 1) {
                myModal.find('#first , #next , #previous , #last').prop('disabled', true);
                //disable all
            }
            else if (first == 1 && last > 1) {
                myModal.find('#next , #last').prop('disabled', false);
                myModal.find('#first , #previous').prop('disabled', true);
                //disable first & prev
            }
            else if (first > 1 && first != last) {
                myModal.find('#first , #next , #previous , #last').prop('disabled', false);
                //enable all
            }
            else if (first > 1 && first == last) {
                myModal.find('#first , #previous').prop('disabled', false);
                myModal.find('#next , #last').prop('disabled', true);
            }
            else {
                myModal.find('#first , #next , #previous , #last').prop('disabled', true);
                //disable all
            }
        },
        error: function (xhr, status, error) {
            if (error == '') { closeFch(); alert('NETWORK ERROR\n\nCheck your connection and try again'); }
        }
    });

    //The following handles click, dblClick and the return of value to the user
    myModal.find('.djq-body2 > ' + tableID).off('click', 'tr'); //To remove events misbehaviour
    myModal.find(' .select-dataJQ, #first , #next , #previous , #last').off('click'); //To remove events misbehaviour when the select
    //button on the modal is clicked
    btnSelect.prop('disabled', true);

    myModal.find('#first').on('click', function () {
        var ss = searchTerm.split('~');
        CreateDynamicDT1(actionLink, ss[0] + '~1',
            tableHeadArray, dbColNamesArray, dbParentName, myModal, tableID, dataReturnID, colIDToReturn, opsPara, callback);
    });

    myModal.find('#previous').on('click', function () {
        var ss = searchTerm.split('~');
        var pageNum = myModal.find('#djq-curVal').html(); pageNum = pageNum.trim(); pageNum = pageNum / 1; pageNum--;
        CreateDynamicDT1(actionLink, ss[0] + '~' + pageNum,
            tableHeadArray, dbColNamesArray, dbParentName, myModal, tableID, dataReturnID, colIDToReturn, opsPara, callback);
    });

    myModal.find('#next').on('click', function () {
        var ss = searchTerm.split('~');
        var pageNum = myModal.find('#djq-curVal').html(); pageNum = pageNum.trim(); pageNum = pageNum / 1; pageNum++;
        CreateDynamicDT1(actionLink, ss[0] + '~' + pageNum,
            tableHeadArray, dbColNamesArray, dbParentName, myModal, tableID, dataReturnID, colIDToReturn, opsPara, callback);
    });

    myModal.find('#last').on('click', function () {
        var ss = searchTerm.split('~');
        CreateDynamicDT1(actionLink, ss[0] + '~' + (myModal.find('#djq-max').html()),
            tableHeadArray, dbColNamesArray, dbParentName, myModal, tableID, dataReturnID, colIDToReturn, opsPara, callback);
    });

    myModal.find(table2).on('click', 'tr', function () {

        if ($(this).hasClass(cssClassName)) {
            $(this).removeClass(cssClassName);
            myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
            btnSelect.prop('disabled', true);
        }
        else {
            myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
            $(this).addClass(cssClassName);
            btnSelect.prop('disabled', false);
        }
    });

    var dataReturn;
    var dRID = $(dataReturnID);
    myModal.find(table2).on('dblclick', 'tr', function () {

        //assign all row data to id='rowData'
        var rrData = ''; var j = 0;
        for (var jj = 0; jj < tableHeadArray.length + 1; jj++) {
            if (j > 0) {
                rrData += '++++ ' + $(this).find('td:nth-child(' + ++j + ')').html();
            }
            else {
                rrData = $(this).find('td:nth-child(' + ++j + ')').html();
            }
        }
        myModal.find('#rowData').val(rrData);

        //Fill multiple fields
        if (Array.isArray(colIDToReturn)) {
            var ii = 1;
            for (var index in colIDToReturn) {
                var colData = $(this).find('td:nth-child(' + colIDToReturn[index] + ')').text();

                $(dataReturnID + ii).hasClass('eDrp') ?
                        $(dataReturnID + ii + ' >option[value="' + colData + '"]').prop('selected', true)
                :
                $(dataReturnID + ii)[0].hasAttribute('value') ?
                        $(dataReturnID + ii).val(colData) : $(dataReturnID + ii).text(colData);
                ii++;
            }
            myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
            btnSelect.prop('disabled', true);

            myModal.find('.close-dataJQ').click();
            //myModal.addClass('djq-hide');
            //$('body').removeClass('djq-vo');

            if (typeof callback == "function") { callback(); }
            $(dataReturnID + '1').focus();
        }
            //Fill one field
        else {
            dataReturn = $(this).find('td:nth-child(' + colIDToReturn + ')').text();

            myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
            btnSelect.prop('disabled', true);

            myModal.find('.close-dataJQ').click();
            //myModal.addClass('djq-hide');
            //$('body').removeClass('djq-vo');
            dRID.val(dataReturn);

            if (typeof callback == "function") { callback(dataReturn); }
            dRID.focus();
        }
    });

    btnSelect.on('click', function () {

        //assign all row data to id='rowData'
        var rrData = ''; var j = 0;
        for (var jj = 0; jj < tableHeadArray.length + 1; jj++) {
            if (j > 0) {
                rrData += '++++ ' + myModal.find(table2 + ' .' + cssClassName + ' td:nth-child(' + ++j + ')').html();
            }
            else {
                rrData = myModal.find(table2 + ' .' + cssClassName + ' td:nth-child(' + ++j + ')').html();
            }
        }
        myModal.find('#rowData').val(rrData);

        //Fill multiple fields
        if (Array.isArray(colIDToReturn)) {
            var ii = 1;
            for (var index in colIDToReturn) {
                var colData = myModal.find(table2 + ' .' + cssClassName + ' td:nth-child(' + colIDToReturn[index] + ')').text();

                $(dataReturnID + ii)[0].hasAttribute('value') ?
                        $(dataReturnID + ii).val(colData) : $(dataReturnID + ii).text(colData);

                ii++;
            }
            myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
            btnSelect.prop('disabled', true);

            myModal.find('.close-dataJQ').click();
            //myModal.addClass('djq-hide');
            //$('body').removeClass('djq-vo');

            if (typeof callback == "function") { callback(); }
            $(dataReturnID + '1').focus();
        }

            //Fill one field
        else {
            dataReturn = myModal.find(table2 + ' .' + cssClassName + ' td:nth-child(' + colIDToReturn + ')').text();

            myModal.find(table2 + ' tbody tr').removeClass(cssClassName);
            btnSelect.prop('disabled', true);

            myModal.find('.close-dataJQ').click();
            //myModal.addClass('djq-hide');
            //$('body').removeClass('djq-vo');
            dRID.val(dataReturn);

            if (typeof callback == "function") { callback(dataReturn); }
            dRID.focus();
        }
    });

    myModal.find("#djq-aval").addClass('aval hide');
};

/*
    asynchronous =false, means that the user interface will freeze until the ajax request finishes
    executing on the server.
    UserExists is good but not perfect because it only allows parameters to be passed through the link
    and not through an object.
    Preferably, SendBack should always be used for such requests because thats their only difference
    except that sendback is always async:true
This function checks if a user exists on the server and returns 'True' else 'False'
*/  function UserExists(/*Pass the link to the url '/controllerName/actionName/2'
*/ actionLink) {

    var token = $('input[name = "__RequestVerificationToken"]').val();

    var aLink = actionLink.split('/');
    actionLink = "/" + aLink[1] + "/" + aLink[2];
    aLink = aLink[3];

    var rett = 'False';
    $.ajax({
        type: "POST",
        url: actionLink,
        data: { __RequestVerificationToken: token, Id: aLink },
        success: function (data, status, xhr) { rett = data; },
        async: false,/*means pause the user interface until ajax comesback i.e execute js codes sequentially*/
        error: function (xhr, status, error) { alert('NETWORK ERROR\n\nCheck your connection and try again'); }
    });
    return rett;
};

/*
    asynchronous =true

    UserExists is good but not perfect because it only allows parameters to be passed through the link
    and not through an object.
    Preferably, SendBack should always be used for such requests because thats their only difference
    except that sendback is always async:true
This function checks if a user exists on the server and returns 'True' else 'False'
*/  function UserExists2(/*Pass the link to the url '/controllerName/actionName/2'
*/ actionLink, callback) {
    openFch();
    var token = $('input[name = "__RequestVerificationToken"]').val();

    var aLink = actionLink.split('/');
    actionLink = "/" + aLink[1] + "/" + aLink[2];
    aLink = aLink[3];

    var rett = 'False';
    $.ajax({
        type: "POST",
        url: actionLink,
        data: { __RequestVerificationToken: token, Id: aLink },
        success: function (data, status, xhr) {
            rett = data;
            if (typeof callback == "function") { closeFch(); callback(rett); }
        },
        error: function (xhr, status, error) {
            if (error == '') { closeFch(); alert('NETWORK ERROR\n\nCheck your connection and try again'); }
        }
    });
};

/*
This function is used for entity framework viewModels
This function reads data from the Database and fills up specific controls that have being marked to be filled up.
All Controls that are to be automatically filled are marked using the general class [edit]
 and for specificity sake, controls are also marked using class names such as [eChk] for CheckBox,
 [eTxt] for TextBox, [eDte] for DateBox and [eNum] for NumberBox.
Tables within the form are also automatically filled up if they've been marked with constant Id's such as FillUpTable1,
 FillUpTable2, FillUpTable3 etc..
The content of such tables are usually generated on the server and deconstructed on the client using separators and
 starting names. e.g the server passes =@"
            <tr>
                <td>aa</td>
                <td>bb</td>
             </tr>
        ++++<tr>
                <td>aa</td>
                <td>bb</td>
             </tr>
        ++++<tr>
                <td>aa</td>
                <td>bb</td>
             </tr>"
 JavaScript will deconstruct the above into 3 arrays using 4 'pluses'(++++) as a diffrentiator,
        pick each array, search for the a table that has a tag of '#FillUpTable-1-2-3-etc' and insert
        the correct contents thats in the picked array into the body of the found table..
        Repeat the above for the remaining two arrays.

*/  function FillUp1(/* /AJAX/CIMGet/1532 */actionLink,
/* dOM Variable */dOM,
/* formId i.e. #mainForm with the hash tag*/formId,
/* CustomerIMT without dot */defaultClassName,
/* Pass 'True' or 'False' */ mustFillUpTable) {

    var token = $('input[name = "__RequestVerificationToken"]').val();

    var aLink = actionLink.split('/');
    actionLink = "/" + aLink[1] + "/" + aLink[2];
    aLink = aLink[3];

    $.ajax({
        type: "POST",
        url: actionLink,
        data: { __RequestVerificationToken: token, Id: aLink },
        success: function (data, status, xhr) {

            if (data == '[object Object]') {

                $.each(data, function (i, data) {
                    var editName = '';
                    var nameVal = '';

                    dOM.find(formId + ' .edit').each(function () {

                        //console.log(dOM.find(formId + ' .edit'));
                        editName = $(this).attr('name');
                        nameVal = '"' + editName + '"';
                        editName = editName.split(".");

                        if (editName[0] == defaultClassName && data[editName[1]] != 'null' && data[editName[1]] != 'undefined') {
                            if ($(this).hasClass("eTxt")) {
                                dOM.find('[name=' + nameVal + ']').val(data[editName[1]]);
                            }
                            else if ($(this).hasClass("eChk")) {
                                dOM.find('[name=' + nameVal + ']').prop('checked', data[editName[1]]);
                            }
                            else if ($(this).hasClass("eNum")) {
                                dOM.find('[name=' + nameVal + ']').val(data[editName[1]].toFixed(2));
                            }
                            else if ($(this).hasClass("eDte")) {
                                dOM.find('[name=' + nameVal + ']').val(formatDate2(data[editName[1]]));
                            }
                        }
                        else {
                            try {
                                var errorCatch = data[editName[0]][editName[1]];

                                if (data[editName[0]][editName[1]] != 'null' && data[editName[0]][editName[1]] != 'undefined') {
                                    if ($(this).hasClass("eTxt")) {
                                        dOM.find('[name=' + nameVal + ']').val(data[editName[0]][editName[1]]);
                                    }
                                    else if ($(this).hasClass("eChk")) {
                                        dOM.find('[name=' + nameVal + ']').prop('checked', data[editName[0]][editName[1]]);
                                    }
                                    else if ($(this).hasClass("eNum")) {
                                        dOM.find('[name=' + nameVal + ']').val(data[editName[0]][editName[1]].toFixed(2));
                                    }
                                    else if ($(this).hasClass("eDte")) {
                                        dOM.find('[name=' + nameVal + ']').val(formatDate2(data[editName[0]][editName[1]]));
                                    }
                                }
                            }
                            catch (error) {
                                //alert(nameVal + '------' + error);
                            }
                        }

                        if (mustFillUpTable == 'True' &&
                            data['FillUpTable'].trim() != "" &&
                            data['FillUpTable'] != 'null' &&
                            data['FillUpTable'] != 'undefined') {
                            editName = data['FillUpTable'];
                            editName = editName.split("++++");
                            var ii = 1;
                            for (var arrayIndex in editName) {
                                dOM.find('#FillUpTable' + ii + ' tbody').html(editName[arrayIndex]);
                                ii++;
                            };
                        }
                    });

                    /*
                    dOM.find('#txtReference').val(data.Id);
                    dOM.find('#txtFullName').val(data.Name);
                    dOM.find('#txtClassification').val(data.ClassificationId);
                    dOM.find('#txtAddress1').val(data.Address1);
                    dOM.find('#txtAddress2').val(data.Address2);
                    dOM.find('#txtTradeZone').val(data.TradeZoneId);
                    dOM.find('#txtState').val(data.StateId);
                    dOM.find('#txtCountry').val(data.CountryId);
                    dOM.find('#txtPhone').val(data.Phone);
                    dOM.find('#txtEmailAddress').val(data.EmailAddress);
                    dOM.find('#txtContactPerson').val(data.ContactPerson);
                    dOM.find('#txtDateRegistered').val(formatDate2(data.DateRegistered));
                    dOM.find('#txtAcctsOfficer1').val(data.AcctsOfficerId);
                    if (data.AcctsOfficerId != null) { dOM.find('#txtAcctsOfficer2').val(data.AcctsOfficer.Name); }
                    dOM.find('#CustomerIMT_ProductPriceProfileId').val(data.ProductPriceProfile.Id);
                    dOM.find('#CustomerIMT_RoyaltyRewardTypeId').val(data.RoyaltyRewardType.Id);
                    dOM.find('#CustomerIMT_DiscountCardHolderAcc').prop('checked', data.DiscountCardHolderAcc);
                    dOM.find('#CustomerIMT_NonCreditClient').prop('checked', data.NonCreditClient);
                    dOM.find('#CustomerIMT_WholesaleClient').prop('checked', data.WholesaleClient);
                    dOM.find('#CustomerIMT_TrackPrevious').prop('checked', data.TrackPrevious);
                    dOM.find('#CustomerIMT_CurrencyId').val(data.Currency.Id);
            
                    dOM.find('#txtMinimum').val(data.SpecialPriceTable.Minimum.toFixed(2));
                    dOM.find('#txtMaximum').val(data.SpecialPriceTable.Maximum.toFixed(2));
                    dOM.find('#txtMarkUp').val(data.SpecialPriceTable.PercentageMarkUpOnCost.toFixed(2));
            
                    dOM.find('#txtCurrentDebit').val(data.AccountInformation.CurrentDebit.toFixed(2));
                    dOM.find('#txtCurrentCredit').val(data.AccountInformation.CurrentCredit.toFixed(2));
                    dOM.find('#txtBalance').val(data.AccountInformation.Balance.toFixed(2));
                    dOM.find('#txtCreditLimit').val(data.AccountInformation.CreditLimit.toFixed(2));
                    dOM.find('#txtAvailableCredit').val(data.AccountInformation.AvailableCredit.toFixed(2));
                    dOM.find('#txtDefaultGLLinkCode1').val(data.GLlinkCodeId);
                    if (data.GLlinkCodeId != null) { dOM.find('#txtDefaultGLLinkCode2').val(data.GLlinkCode.Name); }
                    */
                });
            }
        },
        error: function (xhr, status, error) { alert('NETWORK ERROR\n\nCheck your connection and try again'); }
    });
};

/*
    This function is used for ERP view models where classes/tables are not internally linked to other tables using 
    entity frameworks linking convention
    
    This function reads data from the Database and fills up specific controls that have being marked to be filled up.
    
    All Controls that are to be automatically filled are marked using the general class [edit]
    and for specificity sake, controls are also marked using class names such as [eChk] for CheckBox,
    [eTxt] for TextBox, [eDte] for DateBox and [eNum] for NumberBox

    Asides the above, all classes to be filled must be assigned their proper names according to the class fields 
    that was filled on the server 
    e.g.. <input type="text" name="CUSTOMER.REFERENCE" />
    on the server.. var vm = new MR_DATA.MR_DATAvm { CUSTOMER=new MR_DATA.CUSTOMER{REFERENCE="00001"}, etc.. };
    

    If a table is to be filled up, its data is to be stored in --------- SYSCODETABSvm.ERPmiscl.FillUpTable [TAKE NOTE]
    Tables within the form are also automatically filled up if they've been marked with constant Id's such as FillUpTable1,
    FillUpTable2, FillUpTable3 etc..
    
    The content of such tables are usually generated on the server and deconstructed on the client using separators and
    starting names. e.g the server passes =@"
            <tr>
                <td>aa</td>
                <td>bb</td>
             </tr>
        ++++<tr>
                <td>aa</td>
                <td>bb</td>
             </tr>
        ++++<tr>
                <td>aa</td>
                <td>bb</td>
             </tr>"
 JavaScript will deconstruct the above into 3 arrays using 4 'pluses'(++++) as a diffrentiator,
        pick each array, search for the a table that has a tag of '#FillUpTable-1-2-3-etc' and insert
        the correct contents thats in the picked array into the body of the found table..
        Repeat the above for the remaining two arrays.

*/  function FillUp2(/* /AJAX/CIMGet/1532 */actionLink,
/* dOM Variable */dOM,
/* formId i.e. #mainForm with the hash tag*/formId,
/* Pass 'True' or 'False' */ mustFillUpTable, callback) {

    openFch();
    var token = $('input[name = "__RequestVerificationToken"]').val();

    var aLink = actionLink.split('/');
    actionLink = "/" + aLink[1] + "/" + aLink[2];
    aLink = aLink[3];

    $.ajax({
        type: "POST",
        url: actionLink,
        data: { __RequestVerificationToken: token, Id: aLink },
        success: function (data, status, xhr) {

            closeFch();
            if (data == '[object Object]') {

                var editName = '';
                var nameVal = '';

                dOM.find(formId + ' .edit').each(function () {
                    editName = $(this).attr('name');
                    nameVal = '"' + editName + '"';
                    editName = editName.split(".");

                    try {
                        var errorCatch = data[editName[0]][editName[1]];

                        if (errorCatch == "[object Object]") {
                            var curVal = data[editName[0]][editName[1]][editName[2]];
                            if ($(this).hasClass("eTxt")) {

                                $(this)[0].hasAttribute('value') ?
                                        dOM.find('[name=' + nameVal + ']').val(curVal) : dOM.find('[name=' + nameVal + ']').text(curVal);
                            }
                            else if ($(this).hasClass("eChk")) {
                                dOM.find('[name=' + nameVal + ']').prop('checked', curVal);
                            }
                            else if ($(this).hasClass("eNum")) {
                                dOM.find('[name=' + nameVal + ']').val(curVal.toFixed(2));
                            }
                            else if ($(this).hasClass("eDte")) {
                                dOM.find('[name=' + nameVal + ']').val(formatDate2(curVal));
                            }
                        }
                        else if (errorCatch != null && errorCatch != undefined) {
                            if ($(this).hasClass("eTxt")) {

                                $(this)[0].hasAttribute('value') ?
                                        dOM.find('[name=' + nameVal + ']').val(errorCatch) : dOM.find('[name=' + nameVal + ']').text(errorCatch);
                            }
                            else if ($(this).hasClass("eChk")) {
                                dOM.find('[name=' + nameVal + ']').prop('checked', errorCatch);
                            }
                            else if ($(this).hasClass("eNum")) {
                                dOM.find('[name=' + nameVal + ']').val(errorCatch.toFixed(2));
                            }
                            else if ($(this).hasClass("eDte")) {
                                dOM.find('[name=' + nameVal + ']').val(formatDate2(errorCatch));
                            }
                        }
                    }
                    catch (error) {
                        //alert(nameVal + '------' + error);
                    }
                });

                if (mustFillUpTable == 'True' /*&&
                data['FillUpTable'].trim() != "" &&
                data['FillUpTable'] != 'null' &&
                data['FillUpTable'] != 'undefined'*/) {
                    editName = data['SYSCODETABSvm']['ERPmiscl']['FillUpTable'];
                    editName = editName.split("++++");
                    var ii = 1;
                    for (var arrayIndex in editName) {
                        dOM.find('#FillUpTable' + ii + ' tbody').html(editName[arrayIndex]);
                        ii++;
                    };
                }
            }
            if (typeof callback == "function") { callback(data); }
        },
        error: function (xhr, status, error) {
            closeFch(); alert('NETWORK ERROR\n\nCheck your connection and try again');
        }
    });
};

function ShowModal(dOM, modalCaller, modalID, callback) {
    /*
      how to use ShowModal()
      pass the following to ShowModal using the following
      -dOM is a var holding the entire html of the socument object model $('body')
      -modalCaller is the id of the button that calls up the modal html content
      -modalID is the id of the created modal ie.. the parent
      -callback is a function thats passed to this ShowModal function, to be called 
       after the modal has shown. it can contain codes to be executed strictly for
       the assigned modal
    */


    dOM.find(modalCaller).on({//ON CLICK OF
        click: function () {
            var myModal = dOM.find(modalID); //dataLkUp
            $('body').addClass('djq-vo');
            myModal.removeClass('djq-hide');

            myModal.find('.close-dataJQ').on({
                click: function () {
                    myModal.addClass('djq-hide');
                    $('body').removeClass('djq-vo');
                }
            });

            if (typeof callback == "function") { callback(/*rett*/); }
        }
    });
};

/*
This function checks if a user exists on the server and returns 'True' else 'False'
or whateveer you choose
*/  function SendBack(/*Pass the link to the url '/controllerName/actionName'
*/ actionLink, params, callback) {
    /*
    how to use
    -actionLink is used as a url variable and passed through
     c# mvc's "@Url.Action("MrAttendExists", "AJAX")" which is later converted by
     razor syntax to "~/AJAX/MrAttendExists"
    -params is an object that's passed in the form of key value pairs as { Id:"lol", Name:"2" }
     which is later used in the methods on the server as a parameter e.g MrAttendExists(string Id, int Name)
    -callback is a function thats optional but is used as follows
            SendBack( a, b, function nameOfCallBackWhatever(cd) {
            
            cd is a var that can hold a json object or whatever you pass to it from the server,
            but can only be used within the callback
            
            }
    */

    openFch();

    params.__RequestVerificationToken = $('input[name = "__RequestVerificationToken"]').val();

    actionLink = actionLink.split('/');
    actionLink = "/" + actionLink[1] + "/" + actionLink[2];

    var rett = 'False';
    $.ajax({
        type: "POST",
        url: actionLink,
        data: params,
        success: function (data, status, xhr) {
            rett = data;
            if (typeof callback == "function") { closeFch(); callback(rett); }
        },
        error: function (xhr, status, error) {
            if (error == '') { closeFch(); alert('NETWORK ERROR\n\nCheck your connection and try again'); }
        }
    });
};








/*
This function is mostly fired on the click event of the Clear Button
What it mostly does is to reload the page and scroll to the top
*/
function Refresh() {
    location.reload(true);
    scrollToTop();
};

/*
This is used to alert the user that an error occured on the server
*/
function servErrorAlert(eLookUpID, /*Pass A@T ViewBag.Msg */ exMessage) {
    var myModal = $(eLookUpID);

    if (exMessage != '') {
        $('body').addClass('djq-vo');
        myModal.removeClass('djq-hide');

        myModal.find('.close-dataJQ').on({
            click: function () {
                myModal.addClass('djq-hide');
                $('body').removeClass('djq-vo');
            }
        });
    }

};

function appendTagInvalid1(
/*Pass $('body') as dOM*/ dOM,
/*Pass @ViewBag.iinvalid as invNames i.e invalid control names VCTRANS.RECID,VCTRANS.NAME etc*/ invNames) {
    if (invNames != '') {
        var name = "";
        for (var i = 0; i < invNames.length; i++) {
            if (invNames.charAt(i) == ',') {
                dOM.find("[name='" + name + "']").addClass('iinvalid1');
                name = "";
            }
            else { name += invNames.charAt(i); }
        };
    }
};

/* This function is used to set and lose focus on a reference textbox when the submit
     button is clicked and the server returns data back to the user
     but especially when there is a table to be filled up based on the reference present
         therfore it automatically sets focus and loses focus on that particular control in order to artifficially
         fetch the data thats needed when lose focus occurs.
*/
function setLoseFocus(dOM, refID) {
    if (dOM.find(refID).val().trim != '') {
        dOM.find(refID).focus();
        dOM.find(refID).focusout();
    }
};

function disableEnterKey(dOM, refID) {
    dOM.find(refID).keypress(function (e) {
        if (e.which == 13) {
            return false;
        }
    });
};

function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};

function formatDate1(str) {
    str = str.indexOf('(') != -1 ?
    Number(str.slice(str.indexOf("(") + 1, str.indexOf(")")))
    :
    str;
    var d = new Date(str);
    d = ('0' + d.getDate()).slice(-2) + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + d.getFullYear();
    return d;//returns 01-01-1970 str parameter is either a time in millisecods like (654654645108) or date like 01-01-2017
};

function formatDate2(str) {
    str = str.indexOf('(') != -1 ?
    Number(str.slice(str.indexOf("(") + 1, str.indexOf(")")))
    :
    str;
    var d = new Date(str);
    d = d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);
    return d;//returns 1970-01-01
};

function formatDate3(str) {
    str = str.indexOf('(') != -1 ?
    Number(str.slice(str.indexOf("(") + 1, str.indexOf(")")))
    :
    str;
    var d = new Date(str);
    d = d.getFullYear() + '-' + ('0' + d.getDate()).slice(-2) + '-' + ('0' + (d.getMonth() + 1)).slice(-2);
    return d;//returns 1970-01-01
};

function GetDayInYear(dateVal) {
    var nd = new Date(dateVal);

    var aa = 0;
    var mv = nd.getMonth() + 1;
    var yv = nd.getFullYear();

    if (mv > 2) {
        for (var i = 1; i < mv; i++) {
            aa += i == 1 ? 31
                : i == 2 ? yv % 4 == 0 ? 29 : 28
                : i == 3 ? 31
                : i == 4 ? 30
                : i == 5 ? 31
                : i == 6 ? 30
                : i == 7 ? 31
                : i == 8 ? 31
                : i == 9 ? 30
                : i == 10 ? 31
                : 30;
        }
        aa+=nd.getUTCDate();
    }
    else if (mv > 1) {
        aa = 31 + nd.getUTCDate();
    }

    else { aa = nd.getUTCDate(); }

    return aa;
    /*
    get received month
    calculate previous months
    then add day of current month to prev
    thats the answa
    */
}

function radChkd(radID) {
    a = false;
    if ($(radID + ':checked').length === 1) { a = true; }
    return a;
};

function printPdf(url) {
    var iframe = this._printIframe;
    if (!this._printIframe) {
        iframe = this._printIframe = document.createElement('iframe');
        document.body.appendChild(iframe);

        iframe.style.display = 'none';
        iframe.onload = function () {
            setTimeout(function () {
                iframe.focus();
                iframe.contentWindow.print();
            }, 1);
        };
    }

    iframe.src = url;
};

function openFch() {
    $('body').addClass('djq-vo');
    $('#fchLkUp').removeClass('djq-hide');
};

function closeFch() {
    $('#fchLkUp').addClass('djq-hide');
    $('body').removeClass('djq-vo');
};

function RptBtns(dOM, prnt) {

    if (dOM.find('#pdfPath').val() != null
                && dOM.find('#pdfPath').val().trim() != ''
                && prnt == 'True') {
        printPdf(dOM.find('#pdfPath').val().trim());
    }

    dOM.find('#back1').on({
        click: function () {
            dOM.find('#back2').click();
        },
        dblclick: function () {

            if (dOM.find('#back3').attr('href').trim() != '#')
            { dOM.find('#back3').click(); }
        }
    });

    dOM.find('#print1').on({
        click: function () {

            if (dOM.find('#pdfPath').val() != null
                && dOM.find('#pdfPath').val().trim() != ''
                && dOM.find('#TAB2').hasClass('active')) {

                printPdf(dOM.find('#pdfPath').val().trim());
            }
            else if (dOM.find('#pdfPath').val() != null
                && dOM.find('#pdfPath').val().trim() != ''
                && window.confirm("ALERT!!!\n\nPRINT PREVIOUS REPORT???")) {

                printPdf(dOM.find('#pdfPath').val().trim());
            }
            else {
                dOM.find('#print2').val('True')
                dOM.find('#pre1').click();
            }
        }
    });
};






$(document).ready(function () {

    $.sidebarMenu($('.sidebar-menu'));

    var dOM = $('body'); //must assign

    dOM.find('.header').hover(
	  function () {
	      /*
		  dOM.find('.renderbody-default').animate(
			{"padding-left":"217px"},
			"slow"
		  );
		  */
	      dOM.find('#sbf2').removeClass("hide");
	      dOM.find('.headerr').css({ "display": "block" });
	      dOM.find('.headerr').animate(
			{ "opacity": "0.4" },
			"slow"
		  );
	      $(this).animate(
			  { "width": "327px" },
			  "slow",
			  function () {
			      dOM.find('.header').css({ "overflow-y": "auto" });
			      dOM.find('.header .cblu').addClass("transp");
			  }
		  );
	  },
	  function () {
	      /*
			dOM.find('.renderbody-default').animate(
			{"padding-left":"16px"},
			9, "linear"
		  );
		  */
	      dOM.find('#sbf2').addClass("hide");
	      dOM.find('.headerr').animate(
			{ "opacity": "0" },
			800, "swing",
			function () {
			    dOM.find('.headerr').css({ "display": "none" });
			}
		  );
	      $(this).animate(
			  { "width": "16px" },
			  900, "swing",
			  function () {
			      dOM.find('.header').css({ "overflow-y": "hidden" });
			      dOM.find('.header .cblu').removeClass("transp");
			  }
		  );
	  }
	);

});