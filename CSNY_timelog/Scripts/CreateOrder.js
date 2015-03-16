

$j(function () {


    var today = new Date();
    var y = today.getFullYear().toString().substr(2, 2);

    DefaultArray = [today, "05/26/" + y, "07/04/" + y, "09/01/" + y, "11/27/" + y, "11/28/" + y, "12/25/" + y, "12/26/" + y];
    CustomeArray = [today, "05/26/" + y, "07/04/" + y, "09/01/" + y, "11/27/" + y, "12/25/" + y];

    selectedArray = DefaultArray;

    $j('#BidDate').datepicker({
        selectOtherMonths: true,
        numberOfMonths: 1,
        //dateFormat: "mm/dd/y",
        minDate: '+1D',
        addDisabledDates: [today],
        onSelect: function (dateText) {

        }
    });
});


$j(function () {


    var today = new Date();
    var y = today.getFullYear().toString().substr(2, 2);

    DefaultArray = [today, "05/26/" + y, "07/04/" + y, "09/01/" + y, "11/27/" + y, "12/25/" + y];
    CustomeArray = [today, "05/26/" + y, "07/04/" + y, "09/01/" + y, "11/27/" + y, "11/28/" + y, "12/25/" + y, "12/26/" + y];

    selectedArray = DefaultArray;

    $j('#OnlineDates').datepicker({
        selectOtherMonths: true,
        numberOfMonths: 1,
        //dateFormat: "mm/dd/y",
        minDate: '',
        addDisabledDates: [today],
        onSelect: function (dateText) {

        }
    });
});

function ResetPrintDate() {
    var maxdate;
    var my_date = new Date()
    // maxdate = $('#BidDate').val();

    var TypeOfNotice = $('#TypeOfNotice').val();

    if (TypeOfNotice == "1" || TypeOfNotice == "6") {
        maxdate = $('#BidDate').val();
        maxdate = $("#hidExpireDate").val();
    }
    else {
        maxdate = $("#hidExpireDate").val();
    }

    var today = new Date();
    var y = today.getFullYear().toString().substr(2, 2);

    //var str = "";
    //var DefaultDates = $.trim(str).split(',');

    //var AddDate = "";
    //AddDate = DefaultDates[0];

    $j('#PrintDatesNew').multiDatesPicker('resetDates');
    $j('#PrintDatesNew').datepicker("option", "maxDate", maxdate);
    $j('#PrintDatesNew').multiDatesPicker({
        // altField: '#PrintDates',
        beforeShowDay: $j.datepicker.noWeekends,
        selectOtherMonths: true,
        numberOfMonths: 1,
        minDate: 0,
        dateFormat: "mm/dd/y",
        maxDate: maxdate,        
        addDisabledDates: selectedArray,
        onSelect: function (dateText) {            
            document.getElementById('spnPrintDate').innerHTML = $j('#PrintDatesNew').multiDatesPicker('getDates');
            $j('#PrintDatesNew').datepicker("option", "maxDate", maxdate);
        }
    });

}

function initialize() {
    var maxdate;
    var my_date = new Date()
   // maxdate = $('#BidDate').val();

    var TypeOfNotice = $('#TypeOfNotice').val();

    if (TypeOfNotice == "1" || TypeOfNotice == "6") {
        maxdate = $('#BidDate').val();
        maxdate = $("#hidExpireDate").val();
    }
    else {  
        maxdate = $("#hidExpireDate").val();
    }

    //$j('#PrintDates').datepicker("option", "maxDate", maxdate);

    var today = new Date();
    var y = today.getFullYear().toString().substr(2, 2);

    var str = document.getElementById('spnPrintDate').innerHTML;
    var DefaultDates = $.trim(str).split(',');

    var AddDate = "";
    AddDate = DefaultDates[0];

    if (DefaultDates.length == 2) {
        AddDate = [DefaultDates[0], DefaultDates[1]];
    }
    if (DefaultDates.length == 3) {
        AddDate = [DefaultDates[0], DefaultDates[1], DefaultDates[2]];
    }
    if (DefaultDates.length == 4) {
        AddDate = [DefaultDates[0], DefaultDates[1], DefaultDates[2], DefaultDates[3]];
    }
    if (DefaultDates.length == 5) {
        AddDate = [DefaultDates[0], DefaultDates[1], DefaultDates[2], DefaultDates[3], DefaultDates[4]];
    }
    if (DefaultDates.length == 6) {
        AddDate = [DefaultDates[0], DefaultDates[1], DefaultDates[2], DefaultDates[3], DefaultDates[4], DefaultDates[5]];
    }
    if (DefaultDates.length == 7) {
        AddDate = [DefaultDates[0], DefaultDates[1], DefaultDates[2], DefaultDates[3], DefaultDates[4], DefaultDates[5], DefaultDates[6]];
    }
    if (DefaultDates.length == 8) {
        AddDate = [DefaultDates[0], DefaultDates[1], DefaultDates[2], DefaultDates[3], DefaultDates[4], DefaultDates[5], DefaultDates[6], DefaultDates[7]];
    }
    if (DefaultDates.length == 9) {
        AddDate = [DefaultDates[0], DefaultDates[1], DefaultDates[2], DefaultDates[3], DefaultDates[4], DefaultDates[5], DefaultDates[6], DefaultDates[7], DefaultDates[8]];
    }
    if (DefaultDates.length == 10) {
        AddDate = [DefaultDates[0], DefaultDates[1], DefaultDates[2], DefaultDates[3], DefaultDates[4], DefaultDates[5], DefaultDates[6], DefaultDates[7], DefaultDates[8], DefaultDates[9]];
    }

    $j('#PrintDatesNew').multiDatesPicker('resetDates');
    $j('#PrintDatesNew').datepicker("option", "maxDate", maxdate);
    $j('#PrintDatesNew').multiDatesPicker({
       // altField: '#PrintDates',
        beforeShowDay: $j.datepicker.noWeekends,
        selectOtherMonths: true,
        numberOfMonths: 1,
        minDate: 0,
        dateFormat: "mm/dd/y",
        maxDate: maxdate,
        addDates: AddDate,
        addDisabledDates: selectedArray,
        onSelect: function (dateText) {
            //document.getElementById('spnPrintDate').innerHTML = document.getElementById('getDates').value;
            document.getElementById('spnPrintDate').innerHTML = $j('#PrintDatesNew').multiDatesPicker('getDates');
            $j('#PrintDatesNew').datepicker("option", "maxDate", maxdate);
        }
    });


}

// For Check Ad Type--- Model Binding Start---------------------
$(document).ready(function () {

    var OrderViewModel =
                {
                    Bid: $('#hidNoticeId').val()
                };

    var ShowCompanyName = $('#hidShowCompanyName').val();
    if (ShowCompanyName == 'True') {

        $('#PostingCompanyNameBlock').show();
    }
    else {


        $('#PostingCompanyNameBlock').hide();

    }



    $.ajax({
        type: "POST",
        url: $('#GetAdTypeURL').val(),
        cache: false,
        async: false,
        data: JSON.stringify(OrderViewModel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data.SuccessResult == true) {

                var idshow = $('#hidNoticeId').val() + "s";
                idshow = "#" + idshow;

                if (data.AdType == "Combo Online + Print") {
                    document.getElementById('rbtnCombo').checked = true;

                    $(idshow).hide();
                }
                else if (data.AdType == "Print Only") {
                    document.getElementById('rbtnPrint').checked = true;


                    $('#DivForDistribution').show();
                    $(idshow).hide();
                }
                else if (data.AdType == 'Online Only') {

                    document.getElementById('rbtnOnline').checked = true;


                    $('#DivForDistribution').hide();
                    $(idshow).hide();
                }
                else {
                    document.getElementById('rbtnCombo').checked = true;
                    $('#DivForDistribution').show();
                }

                if (data.BDOQType != null) {

                    var BDOQType = data.BDOQType;
                    var str = BDOQType.split(',');

                    for (var i = 0; i < str.length; i++) {

                        if (str[i] == "1") {

                            document.getElementById('DVBE').checked = true;

                        }
                        else if (str[i] == "2") {
                            document.getElementById('MBE').checked = true;

                        }
                        else if (str[i] == "3") {
                            document.getElementById('WBE').checked = true;

                        }
                        else if (str[i] == "4") {
                            document.getElementById('DBE').checked = true;

                        }
                        else if (str[i] == "5") {
                            document.getElementById('SBE').checked = true;

                        }
                        else if (str[i] == "6") {
                            document.getElementById('OBE').checked = true;

                        }
                        else if (str[i] == "7") {
                            document.getElementById('Other').checked = true;

                        }
                    }
                }
            }

        },
        error: function (xhr) {
            if (xhr.status == 4) {
                //var msg = JSON.parse(xhr.responseText);
                var jsonData = $.parseJSON(xhr.responseText);
                window.location = jsonData.RedirectUrl;
            }

        },
        beforeSend: function () {
            // Handle the beforeSend event
            $('#ajaxbusypanel').show();
        },
        complete: function () {
            // Handle the complete event
            $('#ajaxbusypanel').hide();
        }
    });






    // Check for magazine name Checked 

    var OrderViewModel =
                {
                    Bid: $('#hidNoticeId').val()
                };

    $.ajax({
        type: "POST",
        url: $('#GetMagazineTypeURL').val(),
        cache: false,
        async: false,
        data: JSON.stringify(OrderViewModel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.SuccessResult == true) {

                if (data.MagazinName != "" && data.MagazinName != null) {
                    if (data.MagazinName != "ONLINE")
                    document.getElementById(data.MagazinName).checked = true;
                }


                $("#hidExpireDate").val(data.ExpireDate);

                $("input[name='Checkdaliy']").each(function () {
                    //$(this).attr("checked", "checked");


                    if (data.DailyOption != null) {
                        if (data.DailyOption.indexOf($(this).val()) >= 0) {
                            $(this).attr("checked", "checked");
                        }
                    }

                    //if (str.toLowerCase().indexOf($(this).val()) >= 0) {
                    //    $(this).attr("checked", "checked");
                    //}

                });

                //$(".checkBoxClass").each(function () {
                //    $(this).attr("checked", "checked");
                //});
                toggleCheckState();
            }

        },
        error: function (xhr) {
            if (xhr.status == 4) {
                //var msg = JSON.parse(xhr.responseText);
                var jsonData = $.parseJSON(xhr.responseText);
                window.location = jsonData.RedirectUrl;
            }

        },
        beforeSend: function () {
            // Handle the beforeSend event
            $('#ajaxbusypanel').show();
        },
        complete: function () {
            // Handle the complete event
            $('#ajaxbusypanel').hide();
        }
    });



    // Check for Contact Radio Button
    var OrderViewModel =
                {
                    Bidid: $('#hidNoticeId').val()
                };

    $.ajax({
        type: "POST",
        url: $('#GetContactInfoURL').val(),
        cache: false,
        async: false,
        data: JSON.stringify(OrderViewModel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data.SuccessResult == true) {
                var ss = data;

                if (data.DirectUrl == "" || data.DirectUrl == null) {

                    document.getElementById('rbtnNone').checked = true;
                    // $('#rbtnNone').checked = true;
                    $('#DirectUrl').attr('disabled', 'disabled');
                    $('#EmailAddress').attr('disabled', 'disabled');
                    $('#Fax').attr('disabled', 'disabled');
                }
                else {
                }
            }

        },
        error: function (xhr) {
            if (xhr.status == 4) {
                //var msg = JSON.parse(xhr.responseText);
                var jsonData = $.parseJSON(xhr.responseText);
                window.location = jsonData.RedirectUrl;
            }

        },
        beforeSend: function () {
            // Handle the beforeSend event
            $('#ajaxbusypanel').show();
        },
        complete: function () {
            // Handle the complete event
            $('#ajaxbusypanel').hide();
        }
    });

});

// For Check Ad Type--- Model Binding End ---------------------
// ======================================================================================================

// ----------------- Step - 1 Ad Specification Start ---------------------

var AdSpecification = {


    validate: function () {
        var error = "";

        if ($('#CompanyName').val() == "") {
            error = "Please enter company name.\n";
        }
        if ($('#BidName').val() == "") {
            error = "Please enter notice name.\n";
        }
        var FullName = $('#FullName').val();
        if (FullName == "") {
            error += "Please enter fullname.\n";
        }
        var EMail = $('#Email').val();

        if (EMail == "") {
            error += "Please enter email address.\n";
        }
        else if (EMail.match(/\ /)) {
            error += "Please remove space in email address.\n";
        }
        else if (EMail.indexOf(',') != -1) {
            error += "Please remove comma in email address.\n";
        }
        else {
            var atpos = EMail.indexOf("@");
            var dotpos = EMail.lastIndexOf(".");

            if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= EMail.length) {
                error += "Please provide a valid email address.\n";
            }
        }

        var TypeOfNotice = $('#TypeOfNotice').val();
        if (TypeOfNotice == "") {
            error += "Please select Type Of Notice.\n";
        }

        var CheckBDOQ = false;
        var BDOQValues = "";

        var Name = document.getElementsByName('BDOQType');
        for (var x = 0; x < Name.length; x++) {
            if (Name[x].checked) {
                BDOQValues += Name[x].value + ",";
                CheckBDOQ = true;
            }
        }

        if (TypeOfNotice == "6") {
            if (BDOQValues == "") {
                error += "Please select bdoq type.\n";
            }
        }

        if (TypeOfNotice == "1" || TypeOfNotice == "6") {
            var BidDate = $('#BidDate').val();

            if (BidDate == "Select Bid Date") {
                error += "Please select bid date.\n";
            }
        }

        var StateName = $('#StateName').val();
        if (StateName == "") {
            error += "Please select state name.\n";
        }
        var CitySelect = document.getElementById('City');
        var CityText = CitySelect.options[CitySelect.selectedIndex].text;

        if (CityText == "Select City" || CityText == "-- Select City --") {
            error += "Please select city.\n";
        }

        var Valuation = $('#Valuation').val();
        if (Valuation == "") {
            document.getElementById('DivValuation1').style.display = "none";
            document.getElementById('DivValuation').style.display = "none";
            document.getElementById('DivValuation2').style.display = "none";
        }

        if (error != "") {

            alert(error);
            return false;
        }
        else {
            return true;
        }
    },

    Step1NextButton: function () {

        $('html, body').animate({
            scrollTop: 0
        }, 300);


        if (this.validate()) {

            //Checkout.setLoadWaiting('shipping-method');

            var BDOQValues = "";
            var BDOQText = "";

            var Name = document.getElementsByName('BDOQType');
            for (var x = 0; x < Name.length; x++) {
                if (Name[x].checked) {

                    BDOQValues += Name[x].value + ",";

                    if (Name[x].value == 1) {
                        BDOQText += "DVBE ,  ";
                    }
                    else if ((Name[x].value == 2)) {
                        BDOQText += "MBE ,  ";
                    }
                    else if ((Name[x].value == 3)) {
                        BDOQText += "WBE ,  ";
                    }
                    else if ((Name[x].value == 4)) {
                        BDOQText += "DBE , ";
                    }
                    else if ((Name[x].value == 5)) {
                        BDOQText += "SBE , ";
                    }
                    else if ((Name[x].value == 6)) {
                        BDOQText += "OBE , ";
                    }
                    else {
                        BDOQText += "Other ";
                    }

                }
            }

            if (BDOQText == "") {
                $('#DivForBDOQ1').hide();
                $('#DivForBDOQ').hide();
            }
            else {
                $('#DivForBDOQ1').show();
                $('#DivforBDOQ').show();
                document.getElementById('spanBDOQ1').innerHTML = BDOQText;
                document.getElementById('spanBDOQ').innerHTML = BDOQText;
            }

            if ($('#Valuation').val() == "") {
                $('#DivValuation2').hide();
                $('#DivValuation1').hide();
            }
            else {
                $('#DivValuation2').show();
                $('#DivValuation1').show();
                document.getElementById('spanvaluation').innerHTML = $('#Valuation').val();
                document.getElementById('spanValuation1').innerHTML = $('#Valuation').val();
            }

            var CitySelect = document.getElementById('City');
            var CityText = CitySelect.options[CitySelect.selectedIndex].text;
            document.getElementById('spanPointPersonEmailId').innerHTML = $('#Email').val();
            var OrderViewModel =
                {
                    AdType: $('input[name=AdType]:checked').val(), CompanyName: $('#CompanyName').val(), BidName: $('#BidName').val(),
                    FullName: $('#FullName').val(), Email: $('#Email').val(), InsertionOrder: $('#InsertionOrder').val(),
                    TypeOfNotice: $('#TypeOfNotice').val(), BDOQType: BDOQValues,
                    StateName: $('#StateName').val(), City: CityText,
                    Valuation: $('#Valuation').val(), BidDate: $('#BidDate').val(), PrintDates: document.getElementById('spnPrintDate').innerHTML
                    
                };

            $.ajax({
                type: "POST",
                url: $('#AdSpecificationURL').val(),
                cache: false,
                async: false,
                data: JSON.stringify(OrderViewModel),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {


                    if (data.Message == "Save sucessfully." || data.Message == "update sucessfully.") {
                        var adtype = $('input[name=AdType]:checked').val();

                        document.getElementById('spanPointpersonName').innerHTML = $('#FullName').val();

                        $("#hidExpireDate").val(data.ExpireDate);


                        // For Delete Current Bidid
                        var CurrentBidid = data.Bidid;
                        $('.CurrentBidid').attr('Id', CurrentBidid);

                        if (adtype == "Print Only") {
                            $('#divforonline').hide();
                            document.getElementById('div1step').style.display = "none";
                            document.getElementById('div2step').style.display = "block";
                            document.getElementById('div3step').style.display = "none";
                            document.getElementById('div4step').style.display = "none";
                            document.getElementById('div5step').style.display = "none";
                            document.getElementById('div6step').style.display = "none";

                            // For gray color
                            document.getElementById('12').className = "active";

                            //document.getElementById('2').style.pointerEvents = 'auto';
                            document.getElementById('2').style.display = 'block';
                            document.getElementById('22').style.display = 'none';
                            $('#DivForDistribution').show();
                            $('#DivOnLineSelect').hide();
                            $('#DivNotOnLineSelect').show();
                            $('#DivOnLineSelect1').hide();
                            $('#DivNotOnLineSelect1').show();
                        }
                        else if (adtype == "Online Only") {

                            $('#divforonline').show();
                            $('#divforspec').hide();
                            $('#DivForDistribution').hide();
                            document.getElementById('div1step').style.display = "none";
                            document.getElementById('div2step').style.display = "none";
                            document.getElementById('div3step').style.display = "block";
                            document.getElementById('div4step').style.display = "none";
                            document.getElementById('div5step').style.display = "none";
                            document.getElementById('div6step').style.display = "none";

                            // For gray color
                            document.getElementById('13').className = "active";

                            document.getElementById('2').style.display = 'none';
                            document.getElementById('22').style.display = 'block';

                            document.getElementById('3').style.display = 'block';
                            document.getElementById('33').style.display = 'none';


                            $('#DivOnLineSelect').show();
                            $('#DivNotOnLineSelect').hide();
                            $('#DivOnLineSelect1').show();
                            $('#DivNotOnLineSelect1').hide();
                        }
                        else {

                            $('#divforonline').hide();
                            $('#divforspec').show();
                            $('#DivForDistribution').show();
                            document.getElementById('div1step').style.display = "none";
                            document.getElementById('div2step').style.display = "block";
                            document.getElementById('div3step').style.display = "none";
                            document.getElementById('div4step').style.display = "none";
                            document.getElementById('div5step').style.display = "none";
                            document.getElementById('div6step').style.display = "none";

                            // For gray color
                            document.getElementById('12').className = "active";
                            document.getElementById('2').style.display = 'block';
                            document.getElementById('22').style.display = 'none';

                            $('#DivOnLineSelect').hide();
                            $('#DivNotOnLineSelect').show();
                            $('#DivOnLineSelect1').hide();
                            $('#DivNotOnLineSelect1').show();
                        }
                    }
                    else {

                        alert(data.Message);
                    }
                },
                error: function (xhr) {
                    if (xhr.status == 4) {
                        //var msg = JSON.parse(xhr.responseText);
                        var jsonData = $.parseJSON(xhr.responseText);
                        window.location = jsonData.RedirectUrl;
                    }
                    
                },
                beforeSend: function () {
                    // Handle the beforeSend event
                    $('#ajaxbusypanel').show();
                },
                complete: function () {
                    // Handle the complete event
                    $('#ajaxbusypanel').hide();
                }
            });
        }
    },


    Step1Save: function (response) {
        var BDOQText = "";
        var BDOQValues = "";
        var Name = document.getElementsByName('BDOQType');
        for (var x = 0; x < Name.length; x++) {
            if (Name[x].checked) {
                BDOQValues += Name[x].value + ",";

                if (Name[x].value == 1) {
                    BDOQText += "DVBE , ";
                }
                else if ((Name[x].value == 2)) {
                    BDOQText += "MBE , ";
                }
                else if ((Name[x].value == 3)) {
                    BDOQText += "WBE , ";
                }
                else if ((Name[x].value == 4)) {
                    BDOQText += "DBE , ";
                }
                else if ((Name[x].value == 5)) {
                    BDOQText += "SBE , ";
                }
                else if ((Name[x].value == 6)) {
                    BDOQText += "OBE , ";
                }
                else {
                    BDOQText += "Other , ";
                }

            }
        }
        if (BDOQText == "") {
            $('#DivForBDOQ1').hide();
            $('#DivForBDOQ').hide();
        }
        else {
            $('#DivForBDOQ1').show();
            $('#DivforBDOQ').show();
            document.getElementById('spanBDOQ1').innerHTML = BDOQText;
            document.getElementById('spanBDOQ').innerHTML = BDOQText;
        }


        if ($('#Valuation').val() == "") {
            $('#DivValuation2').hide();
            $('#DivValuation1').hide();
        }
        else {
            $('#DivValuation2').show();
            $('#DivValuation1').show();
            document.getElementById('spanvaluation').innerHTML = $('#Valuation').val();
            document.getElementById('spanValuation1').innerHTML = $('#Valuation').val();
        }

        var CitySelect = document.getElementById('City');
        var CityText = CitySelect.options[CitySelect.selectedIndex].text;

        document.getElementById('spanPointPersonEmailId').innerHTML = $('#Email').val();

        var OrderViewModel =
                {
                    AdType: $('input[name=AdType]:checked').val(), CompanyName: $('#CompanyName').val(), BidName: $('#BidName').val(),
                    FullName: $('#FullName').val(), Email: $('#Email').val(), InsertionOrder: $('#InsertionOrder').val(),
                    TypeOfNotice: $('#TypeOfNotice').val(), BDOQType: BDOQValues,
                    StateName: $('#StateName').val(), City: CityText,
                    Valuation: $('#Valuation').val(), BidDate: $('#BidDate').val(), PrintDates: document.getElementById('spnPrintDate').innerHTML
                    
                };

        $.ajax({
            type: "POST",
            url: $('#AdSpecificationURL').val(),
            cache: false,
            async: false,
            data: JSON.stringify(OrderViewModel),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {


                alert(data.Message);

                if (data.Message == "Save sucessfully." || data.Message == "update sucessfully.") {

                    var adtype = $('input[name=AdType]:checked').val();
                    document.getElementById('spanPointpersonName').innerHTML = $('#FullName').val();


                    // For Delete Current Bidid
                    var CurrentBidid = data.Bidid;
                    $('.CurrentBidid').attr('Id', CurrentBidid);

                    if (adtype == "Print Only") {
                        $('#divforonline').hide();

                        document.getElementById('div1step').style.display = "none";
                        document.getElementById('div2step').style.display = "block";
                        document.getElementById('div3step').style.display = "none";
                        document.getElementById('div4step').style.display = "none";
                        document.getElementById('div5step').style.display = "none";
                        document.getElementById('div6step').style.display = "none";

                        // For gray color
                        document.getElementById('12').className = "active";

                        $('#DivOnLineSelect').hide();
                        $('#DivOnLineSelect3').hide();
                        $('#DivNotOnLineSelect').show();
                        $('#DivOnLineSelect1').hide();
                        $('#DivNotOnLineSelect1').show();
                        $('#DivForDistribution').show();
                        document.getElementById('2').style.display = "block";
                        document.getElementById('22').style.display = "none";

                    }
                    else if (adtype == "Online Only") {
                        $('#divforonline').show();
                        $('#divforspec').hide();

                        document.getElementById('div1step').style.display = "none";
                        document.getElementById('div2step').style.display = "none";
                        document.getElementById('div3step').style.display = "block";
                        document.getElementById('div4step').style.display = "none";
                        document.getElementById('div5step').style.display = "none";
                        document.getElementById('div6step').style.display = "none";

                        // For gray color
                        document.getElementById('13').className = "active";
                        document.getElementById('2').style.display = 'none';
                        document.getElementById('22').style.display = "block";
                        document.getElementById('3').style.display = 'block';
                        document.getElementById('33').style.display = 'none';

                        $('#DivForDistribution').hide();
                        $('#DivOnLineSelect').show();
                        $('#DivOnLineSelect3').show();
                        $('#DivNotOnLineSelect').hide();
                        $('#DivOnLineSelect1').show();
                        $('#DivNotOnLineSelect1').hide();

                    }
                    else {

                        $('#divforonline').hide();
                        $('#divforspec').show();
                        $('#DivForDistribution').show();
                        document.getElementById('div1step').style.display = "none";
                        document.getElementById('div2step').style.display = "block";
                        document.getElementById('div3step').style.display = "none";
                        document.getElementById('div4step').style.display = "none";
                        document.getElementById('div5step').style.display = "none";
                        document.getElementById('div6step').style.display = "none";

                        // For gray color
                        document.getElementById('12').className = "active";

                        // document.getElementById('2').style.pointerEvents = 'auto';
                        document.getElementById('2').style.display = 'block';
                        document.getElementById('22').style.display = 'none';

                        $('#DivOnLineSelect').hide();
                        $('#DivOnLineSelect3').hide();
                        $('#DivNotOnLineSelect').show();
                        $('#DivOnLineSelect1').hide();
                        $('#DivNotOnLineSelect1').show();
                    }
                }
                else {


                }
            },
            error: function (xhr) {
                if (xhr.status == 4) {
                    //var msg = JSON.parse(xhr.responseText);
                    var jsonData = $.parseJSON(xhr.responseText);
                    window.location = jsonData.RedirectUrl;
                }

            },
            beforeSend: function () {
                // Handle the beforeSend event
                $('#ajaxbusypanel').show();
            },
            complete: function () {
                // Handle the complete event
                $('#ajaxbusypanel').hide();
            }
        });
    }
};


    // ----------------- Step - 1 Ad Specification End ---------------------

    // ----------------- Step - 1 Distribution_area Start ---------------------

var DistributionArea = {



    validate: function () {
        var checkfordaily = false;
        var checkvalues = "";
        var Name = document.getElementsByName('Checkdaliy');
        for (var x = 0; x < Name.length; x++) {
            if (Name[x].checked) {
                checkvalues += Name[x].value + ",";
                checkfordaily = true;
            }
        }

        var error = "";
        var disarea = $('input[name=CheckState]:checked').val();

        if (disarea == undefined) {
            error += "Please select notice area.\n";
        }

        var PrintDates = document.getElementById('spnPrintDate').innerHTML;
        //error = "blah";
        if (PrintDates == "") {
            error += "Please select the print dates.\n";
        }

        var datevalue = new Date()
        datevalue.setDate(datevalue.getDate() + 1);

        var splitdates = PrintDates.split(",");

        var i = 0;

        for (var i = 0; i < splitdates.length; i++) {
            var date1 = new Date(splitdates[i]);
                          
        if (datevalue.getMonth() == date1.getMonth()) {

            if (datevalue.getDate() == date1.getDate()) {
                alert("For print in tomorrow's edition, please read the 'Deadline for Print Ads box in the right column. Or call your sales representative for immediate assistance.");

               }
            }
        i++;
        }

        if (error != "") {
            alert(error);
            return false;
        }
        else {
            return true;
        }
    },

    DistributionSave: function () {

        if (this.validate()) {

            var checkfordaily = false;
            var checkvalues = "";
            var Name = document.getElementsByName('Checkdaliy');
            for (var x = 0; x < Name.length; x++) {
                if (Name[x].checked) {
                    checkvalues += Name[x].value + ",";
                    checkfordaily = true;
                }
            }


            var disarea = $('input[name=CheckState]:checked').val();

            var PrintDates = document.getElementById('spnPrintDate').innerHTML;

            var OrderViewModel =
               {
                   StateName: disarea,
                   CheckforDaily: checkfordaily,
                   DailyValues: checkvalues,
                   PrintDates: document.getElementById('spnPrintDate').innerHTML,
                   TypeOfNotice: $('#TypeOfNotice').val(),
                   BidDate: $('#BidDate').val()
               };

            $.ajax({
                type: "POST",
                url: $('#DistributionAreaURL').val(),
                async: false,
                cache: false,
                data: JSON.stringify(OrderViewModel),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.IsError == true) {

                        alert(data.ErrorMessage);
                    }
                    else {
                        document.getElementById('div1step').style.display = "none";
                        document.getElementById('div2step').style.display = "none";
                        document.getElementById('div3step').style.display = "block";
                        document.getElementById('div4step').style.display = "none";
                        document.getElementById('div5step').style.display = "none";
                        document.getElementById('div6step').style.display = "none";

                        // For gray color
                        document.getElementById('13').className = "active";
                        // document.getElementById('3').style.pointerEvents = 'auto';
                        document.getElementById('3').style.display = 'block';
                        document.getElementById('33').style.display = 'none';
                    }

                },
                error: function (xhr) {
                    if (xhr.status == 4) {
                        //var msg = JSON.parse(xhr.responseText);
                        var jsonData = $.parseJSON(xhr.responseText);
                        window.location = jsonData.RedirectUrl;
                    }

                },
                beforeSend: function () {
                    // Handle the beforeSend event
                    $('#ajaxbusypanel').show();
                },
                complete: function () {
                    // Handle the complete event
                    $('#ajaxbusypanel').hide();
                }
            });
        }
    },


    Step2NextButton: function (response) {

        $('html, body').animate({
            scrollTop: 0
        }, 300);

        if (this.validate()) {

            var checkfordaily = false;
            var checkvalues = "";
            var Name = document.getElementsByName('Checkdaliy');
            for (var x = 0; x < Name.length; x++) {
                if (Name[x].checked) {
                    checkvalues += Name[x].value + ",";
                    checkfordaily = true;
                }
            }


            var disarea = $('input[name=CheckState]:checked').val();
            var OrderViewModel =
                    {
                        StateName: disarea,
                        CheckforDaily: checkfordaily,
                        DailyValues: checkvalues,
                        PrintDates: document.getElementById('spnPrintDate').innerHTML,
                        TypeOfNotice: $('#TypeOfNotice').val(),
                        BidDate: $('#BidDate').val()
                    };

            $.ajax({
                type: "POST",
                url: $('#DistributionAreaURL').val(),
                async: false,
                data: JSON.stringify(OrderViewModel),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.IsError == true) {

                        alert(data.ErrorMessage);
                    }
                    else {
                        document.getElementById('div1step').style.display = "none";
                        document.getElementById('div2step').style.display = "none";
                        document.getElementById('div3step').style.display = "block";
                        document.getElementById('div4step').style.display = "none";
                        document.getElementById('div5step').style.display = "none";
                        document.getElementById('div6step').style.display = "none";

                        // For gray color
                        document.getElementById('13').className = "active";
                        // document.getElementById('3').style.pointerEvents = 'auto';
                        document.getElementById('3').style.display = 'block';
                        document.getElementById('33').style.display = 'none';
                    }
                },
                error: function (xhr) {
                    if (xhr.status == 4) {
                        //var msg = JSON.parse(xhr.responseText);
                        var jsonData = $.parseJSON(xhr.responseText);
                        window.location = jsonData.RedirectUrl;
                    }

                },
                beforeSend: function () {
                    // Handle the beforeSend event
                    $('#ajaxbusypanel').show();
                },
                complete: function () {
                    // Handle the complete event
                    $('#ajaxbusypanel').hide();
                }

            });
        }
    }
};

        // ----------------- Step - 2 Distribution_area End ---------------------

        // ----------------- Step - 3 Posting Details Start ---------------------
        
         var PostingDetails = {

             validate: function () {
                 var error = "";


                 if ($('input[name=CompanyType]:checked').val() == "Client") {
                     if ($('select[name=ClientCompanyName]').val() == "") {
                         error = "Please Select Client Company Name.\n";
                     }
                 }

                 if ($('#ProjectName').val() == "") {
                     error = "Please enter Headline.\n";
                     //}
                     //else {
                     //    var letters = /^[A-Za-z\s]+$/;
                     //    if (!$("#ProjectName").val().match(letters)) {
                     //        error += "Project name must be character only.\n";
                     //    }
                 }
                 var Logo = $('#Savedfile_name').val();
                 //if (Logo == "") {
                 //    error += "Please upload logo.\n";
                 //}

                 var Summary = $('#Summary').data("tEditor").value();
                 if (Summary == "") {

                     error += "Please enter Ad Copy.\n";
                 }
                 if ($('input[name=AdType]:checked').val() != "Print Only") {


                     var BidDate = $('#OnlineDates').val();

                     if (BidDate == "Select Online Date") {
                         error += "Please select online notice date.\n";
                     }
                 }

                 var ForConatct = $('input[name=forContact]:checked').val();
                 if (ForConatct == "None") {
                     $('#DivContactIfno').hide();
                 }

                 if (ForConatct == "ContactInfo") {
                     var DirectUrl = $('#DirectUrl').val();

                     var EmailAddress = $('#EmailAddress').val();
                     var Fax = $('#Fax').val();
                     var Url = DirectUrl.substring(0, 3);
                     if (DirectUrl != "" || EmailAddress != "" || Fax != "") {
                         if (DirectUrl != "") {
                             var doturl = DirectUrl.indexOf(".");
                             var letters = /[A-Za-z0-9\.-]{3,}\.[A-Za-z]{3}/;
                            // var letters = /^(http:\/\/www.|https:\/\/www.|ftp:\/\/www.|www.){1}[0-9A-Za-z\.\-]*\.[0-9A-Za-z\.\-]*$/;

                             if (DirectUrl.match(letters)) {

                                 if ("www" == Url.toLocaleString()) {
                                     DirectUrl = "http://" + DirectUrl;
                                 }

                             }
                             else
                                 if ("htt" == Url.toLocaleString()) {
                                     DirectUrl = DirectUrl;
                                 }
                                 else
                                     if (doturl > 0) {
                                         var array = DirectUrl.split(".");
                                         var length = array[1].length;
                                         if (length < 4) {
                                             DirectUrl = "http://www." + DirectUrl;
                                         }
                                         else {
                                             error += "Please enter a valid url.\n";
                                         }
                                     }

                                     else {
                                         error += "Please enter a valid url.\n";
                                     }
                             document.getElementById('spanURl').innerHTML = "<a href='" + DirectUrl + "' target='_blank'>Click Here</a>";
                         }


                         if (EmailAddress != "") {

                             var atpos = EmailAddress.indexOf("@");
                             var dotpos = EmailAddress.lastIndexOf(".");

                             if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= EmailAddress.length) {
                                 error += "Please provide a valid email address.\n";
                             }
                         }

                         if (Fax != "") {
                             var filter = /^[0-9-+]+$/;
                             if (!filter.test(Fax)) {
                                 error += "Please enter valid fax number.\n";
                             }
                         }
                     }
                     else {
                         error += "Please enter at least one field in Contact Information.\n";
                     }
                 }

                 if (error != "") {
                     alert(error);
                     return false;
                 }
                 else {
                     return true;
                 }


             },

            posting_DetailSave: function () {


                if (this.validate()) {

                    var checkstate = "";
                    var Name = document.getElementsByName('CheckState');

                    for (var x = 0; x < Name.length; x++) {
                        if (Name[x].checked) {
                            checkstate += Name[x].value;
                        }
                    }

                    var BDOQValues = "";
                    var Name = document.getElementsByName('BDOQType');
                    for (var x = 0; x < Name.length; x++) {
                        if (Name[x].checked) {
                            BDOQValues += Name[x].value + ",";

                        }
                    }


                    var checkIAgree = false;
                    if ($('#IAgree').checked == true) {
                        checkIAgree = true;
                    }

                    var OrderViewModel =
                    {
                        forContact: $('input[name=forContact]:checked').val(),
                        ProjectName: $('#ProjectName').val(),
                        Logo: $('#Savedfile_name').val(),
                        Summary: $('#Summary').data("tEditor").value(),
                        DirectUrl: $('#DirectUrl').val(),
                        EmailAddress: $('#EmailAddress').val(),
                        Fax: $('#Fax').val(),
                        SpecialInstruction: $('#SpecialInstruction').val(),
                        Processfor: $('input[name=Processfor]:checked').val(),
                        ClientCompanyName: $('select[name=ClientCompanyName]').val(),
                        CompanyType: $('input[name=CompanyType]:checked').val(),
                        OnlineDates: $('#OnlineDates').val(), OnlineDays: $('#OnlineDays').val()
                    };

                    $.ajax({
                        type: "POST",
                        url: $('#PostingDetailsURL').val(),
                        async: false,
                        data: JSON.stringify(OrderViewModel),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {

                            if (data.IsError == false) {

                                // show values step-4  
                                document.getElementById('spanCompanyName').innerHTML = $('#CompanyName').val();

                                document.getElementById('spanInsertion').innerHTML = $('#InsertionOrder').val();
                                document.getElementById('spanAdtype').innerHTML = $('input[name=AdType]:checked').val();
                                var NoticeSelect = document.getElementById('TypeOfNotice');
                                var NoticeText = NoticeSelect.options[NoticeSelect.selectedIndex].text;
                                document.getElementById('spanTypeofnotice').innerHTML = NoticeText;

                                document.getElementById('spanstate').innerHTML = $('#StateName').val();
                                var CitySelect = document.getElementById('City');
                                var CityText = CitySelect.options[CitySelect.selectedIndex].text;
                                document.getElementById('spancity').innerHTML = CityText;
                                document.getElementById('spanvaluation').innerHTML = $('#Valuation').val();
                                document.getElementById('spanBidDate').innerHTML = $('#BidDate').val();
                                document.getElementById('spanPrintDatesShow').innerHTML = document.getElementById('spnPrintDate').innerHTML;

                                document.getElementById('spanProjectName').innerHTML = $('#ProjectName').val();
                                document.getElementById('spanSummry').innerHTML = $('#Summary').data("tEditor").value().substr(0, 130) + '.....';
                                document.getElementById('spanDirectUrl').innerHTML = $('#DirectUrl').val();


                                if (data.ShowPostingCompanyName == true) {
                                    $('#PostingCompanyNameBlock').show();
                                    $('#spanPostingCompanyName').text(data.PostingCompanyName);
                                }
                                else {
                                    $('#PostingCompanyNameBlock').hide();
                                    $('#spanPostingCompanyName').text('');

                                }

                                document.getElementById('spanEmailaddress').innerHTML = $('#EmailAddress').val();
                                document.getElementById('spanfax').innerHTML = $('#Fax').val();
                                document.getElementById('spanMagazine').innerHTML = checkstate;
                                document.getElementById('spanSpecial').innerHTML = $('#SpecialInstruction').val().substr(0, 50) + '.....';

                                document.getElementById('div1step').style.display = "none";
                                document.getElementById('div2step').style.display = "none";
                                document.getElementById('div3step').style.display = "none";
                                document.getElementById('div4step').style.display = "block";
                                document.getElementById('div5step').style.display = "none";
                                document.getElementById('div6step').style.display = "none";

                                // For gray color
                                document.getElementById('14').className = "active";
                                // document.getElementById('4').style.pointerEvents = 'auto';
                                document.getElementById('4').style.display = 'block';
                                document.getElementById('44').style.display = 'none';
                                return false;
                            }
                        },
                        error: function (xhr) {
                            if (xhr.status == 4) {
                                //var msg = JSON.parse(xhr.responseText);
                                var jsonData = $.parseJSON(xhr.responseText);
                                window.location = jsonData.RedirectUrl;
                            }

                        },
                        beforeSend: function () {
                            // Handle the beforeSend event
                            $('#ajaxbusypanel').show();
                        },
                        complete: function () {
                            // Handle the complete event
                            $('#ajaxbusypanel').hide();
                        }
                    });


                }
            },


            Step3NextButton: function (response) {
               
                $('html, body').animate({
                    scrollTop: 0
                }, 300);


                if (this.validate()) {

                    var checkstate = "";
                    var Name = document.getElementsByName('CheckState');
                    for (var x = 0; x < Name.length; x++) {
                        if (Name[x].checked) {
                            checkstate = Name[x].value;
                        }
                    }

                    var checkIAgree = false;
                    if ($('#IAgree').checked == true) {
                        checkIAgree = true;
                    }

                    var OrderViewModel =
                    {
                        forContact: $('input[name=forContact]:checked').val(), ProjectName: $('#ProjectName').val(),
                        Logo: $('#Savedfile_name').val(),
                        Summary: $('#Summary').data("tEditor").value(),
                        DirectUrl: $('#DirectUrl').val(),
                        EmailAddress: $('#EmailAddress').val(),
                        Fax: $('#Fax').val(),
                        SpecialInstruction: $('#SpecialInstruction').val(),
                        Processfor: $('input[name=Processfor]:checked').val(),
                        ClientCompanyName: $('select[name=ClientCompanyName]').val(),
                        CompanyType: $('input[name=CompanyType]:checked').val(),
                        OnlineDates: $('#OnlineDates').val(), OnlineDays: $('#OnlineDays').val()

                    };

                    $.ajax({
                        type: "POST",
                        url: $('#PostingDetailsURL').val(),
                        async: false,
                        data: JSON.stringify(OrderViewModel),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {


                            if (data.IsError == false) {
                                // show values step-4  

                                document.getElementById('spanCompanyName').innerHTML = $('#CompanyName').val();

                                document.getElementById('spanInsertion').innerHTML = $('#InsertionOrder').val();
                                document.getElementById('spanAdtype').innerHTML = $('input[name=AdType]:checked').val();
                                var NoticeSelect = document.getElementById('TypeOfNotice');
                                var NoticeText = NoticeSelect.options[NoticeSelect.selectedIndex].text;
                                document.getElementById('spanTypeofnotice').innerHTML = NoticeText;
                                document.getElementById('spanstate').innerHTML = $('#StateName').val();
                                var CitySelect = document.getElementById('City');
                                var CityText = CitySelect.options[CitySelect.selectedIndex].text;
                                document.getElementById('spancity').innerHTML = CityText;
                                document.getElementById('spanvaluation').innerHTML = $('#Valuation').val();
                                document.getElementById('spanBidDate').innerHTML = $('#BidDate').val();
                                document.getElementById('spanPrintDatesShow').innerHTML = document.getElementById('spnPrintDate').innerHTML;


                                if (data.ShowPostingCompanyName == true) {
                                    $('#PostingCompanyNameBlock').show();
                                    $('#spanPostingCompanyName').text(data.PostingCompanyName);
                                }
                                else {
                                    $('#PostingCompanyNameBlock').hide();
                                    $('#spanPostingCompanyName').text('');

                                }
                                document.getElementById('spanProjectName').innerHTML = $('#ProjectName').val();
                                document.getElementById('spanSummry').innerHTML = $('#Summary').data("tEditor").value().substr(0, 130) + '.....';
                                document.getElementById('spanDirectUrl').innerHTML = $('#DirectUrl').val();
                                document.getElementById('spanEmailaddress').innerHTML = $('#EmailAddress').val();
                                document.getElementById('spanfax').innerHTML = $('#Fax').val();
                                document.getElementById('spanMagazine').innerHTML = checkstate;
                                document.getElementById('spanSpecial').innerHTML = $('#SpecialInstruction').val().substr(0, 50) + '.....';
                                document.getElementById('div1step').style.display = "none";
                                document.getElementById('div2step').style.display = "none";
                                document.getElementById('div3step').style.display = "none";
                                document.getElementById('div4step').style.display = "block";
                                document.getElementById('div5step').style.display = "none";
                                document.getElementById('div6step').style.display = "none";
                                // For gray color
                                document.getElementById('14').className = "active";
                                document.getElementById('4').style.display = 'block';
                                document.getElementById('44').style.display = 'none';
                                return false;
                            }
                        },
                        error: function (xhr) {
                            if (xhr.status == 4) {
                                //var msg = JSON.parse(xhr.responseText);
                                var jsonData = $.parseJSON(xhr.responseText);
                                window.location = jsonData.RedirectUrl;
                            }

                        },
                        beforeSend: function () {
                            // Handle the beforeSend event
                            $('#ajaxbusypanel').show();
                        },
                        complete: function () {
                            // Handle the complete event
                            $('#ajaxbusypanel').hide();
                        }
                    });
                }
            }
        };

        // ----------------- Step - 3 Posting Details Start ---------------------

        // ----------------- Step 4 Review Posting Details Start  ---------------------

        var ReviewPostingDetail = {


            ReviewPostingSave: function () {
                var OrderViewModel =
                {
                    BidName: $('#BidName').val(), OrderDate: $('#BidDate').val(), PrintDates: document.getElementById('spnPrintDate').innerHTML
                };

                $.ajax({
                    type: "POST",
                    url: $('#ReviewPostingURL').val(),
                    async: false,
                    data: JSON.stringify(OrderViewModel),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        alert(data);
                        if (data == "Save Successfully.") {
                            document.getElementById('div1step').style.display = "none";
                            document.getElementById('div2step').style.display = "none";
                            document.getElementById('div3step').style.display = "none";
                            document.getElementById('div4step').style.display = "none";
                            document.getElementById('div5step').style.display = "block";
                            document.getElementById('div6step').style.display = "none";

                            // For gray color
                            document.getElementById('15').className = "active";
                            document.getElementById('5').style.display = 'block';
                            document.getElementById('55').style.display = 'none';
                            document.getElementById('spanProjectNamer').innerHTML = $('#ProjectName').val();
                        }
                    },
                    error: function (xhr) {
                        if (xhr.status == 4) {
                            //var msg = JSON.parse(xhr.responseText);
                            var jsonData = $.parseJSON(xhr.responseText);
                            window.location = jsonData.RedirectUrl;
                        }

                    },
                    beforeSend: function () {
                        // Handle the beforeSend event
                        $('#ajaxbusypanel').show();
                    },
                    complete: function () {
                        // Handle the complete event
                        $('#ajaxbusypanel').hide();
                    }
                });
            },



            Step4NextButton: function (response) {
                
                $('html, body').animate({
                    scrollTop: 0
                }, 300);


                var OrderViewModel =
                {
                    BidName: $('#BidName').val(),
                    OrderDate: $('#BidDate').val(), 
                    PrintDates: document.getElementById('spnPrintDate').innerHTML
                };

                $.ajax({
                    type: "POST",
                    url: $('#ReviewPostingURL').val(),
                    async: false,
                    data: JSON.stringify(OrderViewModel),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data == "Save Successfully.") {
                            document.getElementById('div1step').style.display = "none";
                            document.getElementById('div2step').style.display = "none";
                            document.getElementById('div3step').style.display = "none";
                            document.getElementById('div4step').style.display = "none";
                            document.getElementById('div5step').style.display = "block";
                            document.getElementById('div6step').style.display = "none";

                            // For gray color
                            document.getElementById('15').className = "active";
                            document.getElementById('5').style.display = 'block';
                            document.getElementById('55').style.display = 'none';
                            document.getElementById('spanProjectNamer').innerHTML = $('#ProjectName').val();
                        }

                    },
                    error: function (xhr) {
                        if (xhr.status == 4) {
                            //var msg = JSON.parse(xhr.responseText);
                            var jsonData = $.parseJSON(xhr.responseText);
                            window.location = jsonData.RedirectUrl;
                        }

                    },
                    beforeSend: function () {
                        // Handle the beforeSend event
                        $('#ajaxbusypanel').show();
                    },
                    complete: function () {
                        // Handle the complete event
                        $('#ajaxbusypanel').hide();
                    }
                });
            }
        };

        // ----------------- Step 4 Review Posting Details End ---------------------

        // ----------------- Step 5 SubmitProcessing Start ---------------------

        var SubmitProcessing = {




            SubmitforProcessing: function () {
                var OrderViewModel =
               {
                   AdType: $('input[name=AdType]:checked').val(), Email: $("#Email").val(),
                   FullName: $('#FullName').val(), CompanyName: $('#CompanyName').val(), OrderDate: $('#BidDate').val(),
                   PrintDates: document.getElementById('spnPrintDate').innerHTML, MagazineName: document.getElementById('spanMagazine').innerHTML,
                   EmailAddress: $('#EmailAddress').val()
               };

                $.ajax({
                    type: "POST",
                    url: $('#SendConfirmationURL').val(),
                    async: false,
                    data: JSON.stringify(OrderViewModel),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data.IsError==false) {
                            alert("Submit Success.");

                            if (data.Message == "Submit Success." || data.Message == "Failure sending mail.") {
                                document.getElementById('div1step').style.display = "none";
                                document.getElementById('div2step').style.display = "none";
                                document.getElementById('div3step').style.display = "none";
                                document.getElementById('div4step').style.display = "none";
                                document.getElementById('div5step').style.display = "none";
                                document.getElementById('div6step').style.display = "block";
                                // For gray color
                                document.getElementById('16').className = "active";
                                document.getElementById('6').style.display = 'block';
                                document.getElementById('66').style.display = 'none';

                                document.getElementById('1').style.display = 'none';
                                document.getElementById('111').style.display = 'block';
                                document.getElementById('2').style.display = 'none';
                                document.getElementById('22').style.display = 'block';
                                document.getElementById('3').style.display = 'none';
                                document.getElementById('33').style.display = 'block';
                                document.getElementById('4').style.display = 'none';
                                document.getElementById('44').style.display = 'block';
                                document.getElementById('5').style.display = 'none';
                                document.getElementById('55').style.display = 'block';

                                $('#showbidDetail').hide();
                                $('#showbidName').hide();
                                //document.getElementById('showbidDetail').style.display = 'none';
                                //document.getElementById('showbidName').style.display = 'none';
                            }
                            else {

                                document.getElementById('div1step').style.display = "none";
                                document.getElementById('div2step').style.display = "none";
                                document.getElementById('div3step').style.display = "none";
                                document.getElementById('div4step').style.display = "none";
                                document.getElementById('div5step').style.display = "none";
                                document.getElementById('div6step').style.display = "block";
                                // For gray color
                                document.getElementById('16').className = "active";
                                document.getElementById('6').style.display = 'block';
                                document.getElementById('66').style.display = 'none';
                                document.getElementById('1').style.display = 'none';
                                document.getElementById('111').style.display = 'block';
                                document.getElementById('2').style.display = 'none';
                                document.getElementById('22').style.display = 'block';
                                document.getElementById('3').style.display = 'none';
                                document.getElementById('33').style.display = 'block';
                                document.getElementById('4').style.display = 'none';
                                document.getElementById('44').style.display = 'block';
                                document.getElementById('5').style.display = 'none';
                                document.getElementById('55').style.display = 'block';
                                //document.getElementById('showbidDetail').style.display = 'none';
                                //document.getElementById('showbidName').style.display = 'none';
                                $('#showbidDetail').hide();
                                $('#showbidName').hide();
                            }
                        }
                        else {
                            alert(data.Message); 
                        }
                    },
                    error: function (xhr) {
                        if (xhr.status == 4) {
                            //var msg = JSON.parse(xhr.responseText);
                            var jsonData = $.parseJSON(xhr.responseText);
                            window.location = jsonData.RedirectUrl;
                        }

                    },
                    beforeSend: function () {
                        // Handle the beforeSend event
                        $('#ajaxbusypanel').show();
                    },
                    complete: function () {
                        // Handle the complete event
                        $('#ajaxbusypanel').hide();
                    }
                });
            },



            SaveInCompelete: function () {
                $('html, body').animate({
                    scrollTop: 0
                }, 300);

                var OrderViewModel =
                      {
                          AdType: $('input[name=AdType]:checked').val(), Email: $('#Email').val(),
                          FullName: $('#FullName').val(), CompanyName: $('#CompanyName').val(),
                          PrintDates: document.getElementById('spnPrintDate').innerHTML,
                          MagazineName: document.getElementById('spanMagazine').innerHTML,
                          EmailAddress: $('#EmailAddress').val()
                      };
                $.ajax({
                    type: "POST",
                    url: $('#SaveInCompeleteURL').val(),
                    async: false,
                    data: JSON.stringify(OrderViewModel),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        alert("Saved Successfully as Incomplete Notice.");

                        if (data == "Save Successfully.") {
                            window.location.href = '/account/index';

                        }
                    },
                    error: function (xhr) {
                        if (xhr.status == 4) {
                            //var msg = JSON.parse(xhr.responseText);
                            var jsonData = $.parseJSON(xhr.responseText);
                            window.location = jsonData.RedirectUrl;
                        }

                    },
                    beforeSend: function () {
                        // Handle the beforeSend event
                        $('#ajaxbusypanel').show();
                    },
                    complete: function () {
                        // Handle the complete event
                        $('#ajaxbusypanel').hide();
                    }
                });
            },

            ViewBidDetail: function (e) {
                document.getElementById('bidid').innerHTML = e;
                document.getElementById('div5step').style.display = "none";
                document.getElementById('div4bstep').style.display = "block";
                $.ajax({
                    type: "POST",
                    url: $('#GetBidDetailURL').val(),

                    data: '{Bidid:' + e + ' }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        // alert(data.Logo);
                        document.getElementById('spanAdtypes').innerHTML = data.AdType;
                        document.getElementById('spanTypeofnotices').innerHTML = data.NoticeType;
                        document.getElementById('spanInsertions').innerHTML = data.InsertionOrder;
                        var BDOQType = data.BDOQ;
                        var str = BDOQType.split(',');
                        var BDOQText = ""

                        for (var i = 0; i < str.length; i++) {

                            if (str[i] == "1") {
                                BDOQText += "DVBE , ";
                            }
                            else if (str[i] == "2") {
                                BDOQText += "MBE , ";
                            }
                            else if (str[i] == "3") {
                                BDOQText += "WBE , ";
                            }
                            else if (str[i] == "4") {
                                BDOQText += "DBE , ";
                            }
                            else if (str[i] == "5") {
                                BDOQText += "SBE , ";
                            }
                            else if (str[i] == "6") {
                                BDOQText += "OBE , ";
                            }
                            else if (str[i] == "7") {
                                BDOQText += "Other ,";
                            }
                        }
                        document.getElementById('spanBDOQs').innerHTML = BDOQText;
                        document.getElementById('spanPrintDatesView').innerHTML = data.PrintDates;
                        document.getElementById('spanvaluations').innerHTML = data.Valuation;
                        document.getElementById('spanBidDates').innerHTML = data.BidDate;
                        document.getElementById('spanPrintDatesView').innerHTML = data.PrintDates;
                        document.getElementById('spanstates').innerHTML = data.ProjectLocationState;
                        document.getElementById('spancitys').innerHTML = data.ProjectLocationCity;
                        document.getElementById('spanProjectNames').innerHTML = data.ProjectName;
                        document.getElementById('spanMagazines').innerHTML = data.MagazineName;
                        document.getElementById('spanSummrys').innerHTML = data.Summary.substr(0, 130) + '.....';
                        document.getElementById('spanDirectUrls').innerHTML = data.DirectUrl;
                        document.getElementById('spanEmailaddresss').innerHTML = data.EmailAddress;
                        document.getElementById('spanfaxs').innerHTML = data.Fax;
                        document.getElementById('spanSpecials').innerHTML = data.Fax.substr(0, 50) + '.....';
                        document.getElementById('img_usrimage111').src = data.Logo;
                    },
                    error: function (xhr) {
                        if (xhr.status == 4) {
                            //var msg = JSON.parse(xhr.responseText);
                            var jsonData = $.parseJSON(xhr.responseText);
                            window.location = jsonData.RedirectUrl;
                        }

                    },
                    beforeSend: function () {
                        // Handle the beforeSend event
                        $('#ajaxbusypanel').show();
                    },
                    complete: function () {
                        // Handle the complete event
                        $('#ajaxbusypanel').hide();
                    }
                });

            },

            DeleteBidDetail: function (e) {
                var x;

                var r = confirm("Are you sure want to delete this Notice?");
                if (r == true) {
                    $.ajax({
                        type: "POST",
                        url: $('#DeleteBidURL').val(),
                        data: '{Bidid:' + e + ' }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var idshow = e + "s";
                            idshow = "#" + idshow;
                            $(idshow).hide();
                            alert(data.Message);

                            //                            var id = document.getElementsByClassName('CurrentBidid')[0].id;
                            //                            if (id == e && '@Model.bidid' != 0) {
                            //                                location.href = "~/account/index";
                            //                            }
                            //                            else {
                            //                                if (id == e && '@Model.bidid' == 0 && '@ViewBag.Countbid' == 0) {
                            //                                    location.href = "~/account/index";
                            //                                }
                            //                                else if (id == e && '@Model.bidid' == 0 && '@ViewBag.Countbid' > 0) {
                            //                                    $('#DivCurrentBid').hide(); 
                            //                                }
                            //                                else if (data.Bidcount == 0) {
                            //                                    location.href = "~/account/index";
                            //                                }
                            //                            }
                            //                            if (data.Bidcount == 0) {
                            //                                location.href = "~/account/index";
                            //                            }
                        },
                        error: function (xhr) {
                            if (xhr.status == 4) {
                                //var msg = JSON.parse(xhr.responseText);
                                var jsonData = $.parseJSON(xhr.responseText);
                                window.location = jsonData.RedirectUrl;
                            }

                        },
                        beforeSend: function () {
                            // Handle the beforeSend event
                            $('#ajaxbusypanel').show();
                        },
                        complete: function () {
                            // Handle the complete event
                            $('#ajaxbusypanel').hide();
                        }
                    });
                }

            },

            AddAnotherBid: function (Email, FullName) {

                var x;
                var r = confirm("Click here to add another notice to this order. Either with the same or a different publication.");
                if (r == true) {
                    var OrderViewModel =
                      {
                          AdType: $('input[name=AdType]:checked').val(),
                          EmailAddress: Email,
                          FullName: FullName,
                          BidName: $('#BidName').val()
                      };

                    $.ajax({
                        type: "POST",
                        url: $('#RemoveSessionURL').val(),
                        async: false,
                        data: JSON.stringify(OrderViewModel),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                        },
                        error: function (data) {
                        },
                        beforeSend: function () {
                            // Handle the beforeSend event
                            $('#ajaxbusypanel').show();
                        },
                        complete: function () {
                            // Handle the complete event
                            $('#ajaxbusypanel').hide();
                        }
                    });
                    location.href = $('#createorderURL').val();
                }
                else {
                    return false;
                }
            },

            AddNewBidStep6: function () {

                $.ajax({
                    type: "POST",
                    url: $('#AddNewBidNoticeURL').val()
                });

                location.href = $('#createorderURL').val();
             
            }
        };


        // ----------------- Step 5 SubmitProcessing End ---------------------

        function CheckAndActivation(e) {
            if (e == "1") {
                document.getElementById('div1step').style.display = "block";
                document.getElementById('div2step').style.display = "none";
                document.getElementById('div3step').style.display = "none";
                document.getElementById('div4step').style.display = "none";
                document.getElementById('div5step').style.display = "none";
                document.getElementById('div6step').style.display = "none";
                document.getElementById('div4bstep').style.display = "none";
                document.getElementById('div3bstep').style.display = "none";
                // For gray color
                document.getElementById('11').className = "active";
                return false;
            }
            if (e == "2") {
                document.getElementById('div1step').style.display = "none";
                document.getElementById('div2step').style.display = "block";
                document.getElementById('div3step').style.display = "none";
                document.getElementById('div4step').style.display = "none";
                document.getElementById('div5step').style.display = "none";
                document.getElementById('div6step').style.display = "none";
                document.getElementById('div3bstep').style.display = "none";
                document.getElementById('div4bstep').style.display = "none";
                document.getElementById('12').className = "active";
                //// for disable menus
                document.getElementById('2').style.display = 'block';
                document.getElementById('22').style.display = 'none';
                return false;
            }
            if (e == "3") {
                document.getElementById('div1step').style.display = "none";
                document.getElementById('div2step').style.display = "none";
                document.getElementById('div3step').style.display = "block";
                document.getElementById('div4step').style.display = "none";
                document.getElementById('div5step').style.display = "none";
                document.getElementById('div6step').style.display = "none";
                document.getElementById('div4bstep').style.display = "none";
                document.getElementById('div3bstep').style.display = "none";
                document.getElementById('13').className = "active";
                // for disable menus
                document.getElementById('3').style.display = 'block';
                document.getElementById('33').style.display = 'none';
                return false;
            }
            if (e == "4") {

                document.getElementById('div4bstep').style.display = "none";
                document.getElementById('div1step').style.display = "none";
                document.getElementById('div2step').style.display = "none";
                document.getElementById('div3step').style.display = "none";
                document.getElementById('div4step').style.display = "block";
                document.getElementById('div5step').style.display = "none";
                document.getElementById('div6step').style.display = "none";
                document.getElementById('div3bstep').style.display = "none";
                document.getElementById('14').className = "active";
                // for disable menus
                document.getElementById('4').style.display = 'block';
                document.getElementById('44').style.display = 'none';

                // show values step-4         
                document.getElementById('spanCompanyName').innerHTML = $('#CompanyName').val();
                document.getElementById('spanInsertion').innerHTML = $('#InsertionOrder').val();
                document.getElementById('spanAdtype').innerHTML = $('input[name=AdType]:checked').val();
                var NoticeSelect = document.getElementById("TypeOfNotice");
                var NoticeText = NoticeSelect.options[NoticeSelect.selectedIndex].text;
                $('#spanTypeofnotice').innerHTML = NoticeText;
                document.getElementById('spanBDOQ').innerHTML = BDOQText;
                document.getElementById('spanstate').innerHTML = $('#StateName').val();
                var CitySelect = document.getElementById("City");
                var CityText = CitySelect.options[CitySelect.selectedIndex].text;
                document.getElementById('spancity').innerHTML = CityText;
                document.getElementById('spanvaluation').innerHTML = $('#Valuation').val();
                document.getElementById('spanBidDate').innerHTML = $('#BidDate').val();
                document.getElementById('spanPrintDatesShow').innerHTML = document.getElementById('spnPrintDate').innerHTML;
                document.getElementById('spanProjectName').innerHTML = $('#ProjectName').val();
                document.getElementById('spanSummry').innerHTML = $('#Summary').data("tEditor").value().substr(0, 130) + '.....';
                document.getElementById('spanDirectUrl').innerHTML = $('#DirectUrl').val();
                document.getElementById('spanEmailaddress').innerHTML = $('#EmailAddress').val();
                document.getElementById('spanfax').innerHTML = $('#Fax').val();
                document.getElementById('spanMagazine').innerHTML = $('input[name=CheckState]:checked').val();
                document.getElementById('spanSpecial').innerHTML = $('#SpecialInstruction').val().substr(0, 50) + '.....';
                document.getElementById('spanCompanyNamerr').innerHTML = $('#CompanyName').val();
                return false;
            }
            if (e == "5") {
                document.getElementById('div4bstep').style.display = "none";
                document.getElementById('div1step').style.display = "none";
                document.getElementById('div2step').style.display = "none";
                document.getElementById('div3step').style.display = "none";
                document.getElementById('div4step').style.display = "none";
                document.getElementById('div5step').style.display = "block";
                document.getElementById('div6step').style.display = "none";
                document.getElementById('div3bstep').style.display = "none";
                document.getElementById('15').className = "active";
                // for disable menus
                document.getElementById('5').style.display = 'block';
                document.getElementById('55').style.display = 'none';
                return false;
            }
            if (e == "6") {

                document.getElementById('div4bstep').style.display = "none";
                document.getElementById('div1step').style.display = "none";
                document.getElementById('div2step').style.display = "none";
                document.getElementById('div3step').style.display = "none";
                document.getElementById('div4step').style.display = "none";
                document.getElementById('div5step').style.display = "none";
                document.getElementById('div6step').style.display = "block";
                document.getElementById('div3bstep').style.display = "none";
                // For Disabled Menus
                document.getElementById('16').className = "active";
                document.getElementById('6').style.display = 'block';
                document.getElementById('66').style.display = 'none';
                document.getElementById('1').style.display = 'none';
                document.getElementById('111').style.display = 'block';
                document.getElementById('2').style.display = 'none';
                document.getElementById('22').style.display = 'block';
                document.getElementById('3').style.display = 'none';
                document.getElementById('33').style.display = 'block';
                document.getElementById('4').style.display = 'none';
                document.getElementById('44').style.display = 'block';
                document.getElementById('5').style.display = 'none';
                document.getElementById('55').style.display = 'block';
                return false;
            }

            if (e == "7") {

                document.getElementById('div4bstep').style.display = "none";
                document.getElementById('div1step').style.display = "none";
                document.getElementById('div2step').style.display = "none";
                document.getElementById('div3step').style.display = "none";
                document.getElementById('div4step').style.display = "none";
                document.getElementById('div5step').style.display = "none";
                document.getElementById('div6step').style.display = "none";
                document.getElementById('div3bstep').style.display = "block";
                document.getElementById('13').className = "active";
                // for disable menus
                document.getElementById('3').style.display = 'block';
                document.getElementById('33').style.display = 'none';
                document.getElementById('spanCompanyNamerr').innerHTML = $('#CompanyName').val();
                document.getElementById('spanAdtype').innerHTML = $('input[name=AdType]:checked').val();
                var NoticeSelect = document.getElementById('TypeOfNotice');
                var NoticeText = NoticeSelect.options[NoticeSelect.selectedIndex].text;
                document.getElementById('spanTypeofnotice1').innerHTML = NoticeText;

                $.ajax({
                    type: "POST",
                    url: $('#GetStateAbbrURL').val(),
                    async: false,
                    data: '{StateName: "' + $('#StateName').val() + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        for (var i = 0; i < data.length; i++) {
                            document.getElementById('spanState').innerHTML = data;
                        }
                    },
                    error: function (xhr) {
                        if (xhr.status == 4) {
                            //var msg = JSON.parse(xhr.responseText);
                            var jsonData = $.parseJSON(xhr.responseText);
                            window.location = jsonData.RedirectUrl;
                        }

                    },
                    beforeSend: function () {
                        // Handle the beforeSend event
                        $('#ajaxbusypanel').show();
                    },
                    complete: function () {
                        // Handle the complete event
                        $('#ajaxbusypanel').hide();
                    }
                });
                var CitySelect = document.getElementById('City');
                var CityText = CitySelect.options[CitySelect.selectedIndex].text;
                document.getElementById('spancity1').innerHTML = CityText;
                if ($('#Valuation').val() != "") {
                    document.getElementById('divVal').style.display = "block";
                    document.getElementById('spanValuation1').innerHTML = $('#Valuation').val();
                }
                else {
                    document.getElementById('divVal').style.display = "none";
                }
                document.getElementById('spanBidDate').innerHTML = $('#BidDate').val();
                document.getElementById('spanPrintDatesShow').innerHTML = document.getElementById('spnPrintDate').innerHTML;
                document.getElementById('spanBidDater').innerHTML = $('#BidDate').val();
                document.getElementById('spanProjectName1').innerHTML = $('#ProjectName').val();
                document.getElementById('paragrapshowSummary').innerHTML = $('#Summary').data("tEditor").value();
                if (document.getElementById('rbtnDetail').checked == true) {



                    if ($('#EmailAddress').val() != "") {
                        document.getElementById('spanEmail').innerHTML = $('#EmailAddress').val();
                        document.getElementById('spnLabelEmail').innerHTML = "Contact Email : ";
                        document.getElementById('divContacEmail').style.display = "block";
                        document.getElementById('Divcontact').style.display = "none";
                        document.getElementById('DivcontactUrl').style.display = "block";
                    }
                    else {
                        document.getElementById('divContacEmail').style.display = "none";
                    }
                    if ($('#Fax').val() != "") {
                        document.getElementById('spanFax').innerHTML = $('#Fax').val();
                        document.getElementById('spnLabelFax').innerHTML = "Fax No. : ";
                        document.getElementById('divFax').style.display = "block";
                    }
                    else {
                        document.getElementById('divFax').style.display = "none";
                    }
                    document.getElementById('spnLabelUrl').innerHTML = "Contact URL : ";
                }
                else {
                    document.getElementById('spanURl').innerHTML = "";
                    document.getElementById('spanEmail').innerHTML = "See Notice Summary for details on how to respond to this notice.";
                    document.getElementById('spanFax').innerHTML = "";
                    document.getElementById('spnLabelUrl').innerHTML = "";
                    document.getElementById('spnLabelFax').innerHTML = "";
                    document.getElementById('spnLabelEmail').innerHTML = "Contact : ";
                }
                document.getElementById('spanEmailaddress1').innerHTML = $('#Email').val();
                document.getElementById('paragrapshowSpecialInstruction').innerHTML = $('#SpecialInstruction').val().substr(0, 50) + '.....';
                return false;
            }
            if (e == "8") {
                document.getElementById('div1step').style.display = "none";
                document.getElementById('div2step').style.display = "none";
                document.getElementById('div3step').style.display = "none";
                document.getElementById('div4step').style.display = "none";
                document.getElementById('div5step').style.display = "block";
                document.getElementById('div6step').style.display = "none";
                document.getElementById('div4bstep').style.display = "none";
                return false;
            }
        }