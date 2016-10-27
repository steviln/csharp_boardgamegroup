$(document).ready(function () {

    var dragit = null, prev = null;
    var relX, relY;

    $('.dragField').mousedown(function (e) {
        mouse_it_down(e, $(this));
    });

    $(window).load(function () {
        if ($(document).width() <= 450) {
            $('#mainContent').prepend("<div id='menuLink'><a>Meny</a></div>");
            $('#menuLink').find('a').on("click", function () { $('html, body').animate({ scrollTop: 0},500); });
            $('html, body').animate({
                scrollTop: ($('#mainContent').offset().top)
            }, 500);          
        }
    });




    function mouse_it_down(e, a) {
        dragit = a.parent();
        a.disableSelection();
        var maloffset = dragit.offset();
        relX = e.pageX - maloffset.left;
        relY = e.pageY - maloffset.top;

        prev = dragit.prev();
        dragit.remove();
        dragit.css("position", "absolute");
        dragit.css("z-index", "1000");
        dragit.css("top", (e.pageY - relY) + "px");
        dragit.css("left", (e.pageX - relX) + "px");
        dragit.appendTo($("body"));

    }

    $(document).mouseup(function (e) {
        if (dragit != null) {
            var draguse = dragit;
            dragit = null;
            draguse.removeAttr('style');
            draguse.remove();

            var under = null;
            var offit;
            $('.deltakelseZone').each(function (a) {
                if (!$(this).is(draguse)) {
                    offit = $(this).offset();
                    if (e.pageX > offit.left && e.pageX < (offit.left + $(this).width()) && e.pageY > offit.top && e.pageY < (offit.top + $(this).height())) {
                        under = $(this);
                    }
                }
            });

            if (under == null) {
                prev.before(draguse);
            }
            else {
                var repvalue = parseInt(under.find('.posisjon').find('input').attr('value'));
                if (repvalue <= 0) { repvalue = 1; }
                draguse.find('.posisjon').find('input').val(repvalue);
                under.before(draguse);
                $('.deltakelseZone').each(function (a) {
                    if (!$(this).is(draguse)) {
                        var compvalue = parseInt($(this).find('.posisjon').find('input').attr('value'));
                        if (compvalue >= repvalue) {
                            compvalue++;
                            $(this).find('.posisjon').find('input').val(compvalue);
                        }
                    }
                });

                
            }
            draguse.find('.dragField').on("mousedown", function (e) { mouse_it_down(e, $(this)); });
            prev = null;

        }
    });

    $(document).mousemove(function (e) {
        if (dragit != null) {
            dragit.css("top", (e.pageY - relY) + "px");
            dragit.css("left", (e.pageX - relX) + "px");
        }
    });

    $('#sortButton').click(function () {

        var $sorter = $('#deltakelseZoneHome');
        var $sorteit = $sorter.find('.deltakelseZone').sort(function (a, b) {
            var ab = $(a).find('.poengsum').find('input').attr('value');
            var bb = $(b).find('.poengsum').find('input').attr('value');
            if (!$.isNumeric(ab)) { ab = 0; }
            if (!$.isNumeric(bb)) { bb = 0; }
            ab = parseFloat(ab);
            bb = parseFloat(bb);
            return ab < bb;
        });

        $sorter.append($sorteit);

        var lastsum = null;
        var lastobject = null;
        var thispos = 1;
        var ties = [];

        $('.deltakelseZone').each(function () {
            var pogsum = $(this).find('.poengsum').find('input').attr('value');
            var thisobj = $(this).find('.posisjon').find('input');
            ties.push([thisobj, pogsum]);
            thisobj.val(thispos);
            var spillvalg = $(this).find('.spillerfeltet').find('select').attr('value');
     
            if (spillvalg > -1) {
                if (pogsum == lastsum) {
                    for (var x = ties.length - 1; x >= 0; x--) {
                        if (pogsum == ties[x][1]) {
                            ties[x][0].val(thispos);
                        }
                    }
                }
                lastsum = pogsum;
            }

            thispos++;

        });

    });

    var inputUpdater;

    $('.forgotPlayerButton').click(function () {
        inputUpdater = $(this);
        $('#personUpdaterDiv').css("top", ($(document).scrollTop() + 200) + 'px');
        $('#personUpdaterDiv').css("display","block");
    });

    $('#newPlayerButton').click(function () {
        var firstvar = $('#firstVariable').attr('value');
        var secondvar = $('#secondVariable').attr('value');
        var ajaxdest = inputUpdater.attr('data-ajax-destination');
        $.ajax({
            type: 'GET',
            url: '/Ajax/' + ajaxdest,
            data: { first: firstvar, second: secondvar },
            success: function (data) {
                var returnValues = data.split(";");
                var brukSelecct = inputUpdater.parent().find('select');
                var nypotnew = new Option(returnValues[1], returnValues[0]);
                brukSelecct.append($(nypotnew));
                $(brukSelecct).val(returnValues[0]);
                $('#personUpdaterDiv').css("display", "none");
            }
        });
    });

    $('#cancelNewPlayerButton').click(function () {
        $('#personUpdaterDiv').css("display", "none");
    });

    $('#spillRangeringValg').change(function () {
        var sendval = $(this).val();
        var spillval = $('#modspillID').val();
        $.ajax({
            type: 'GET',
            url: '/Ajax/RateGame/',
            data: { first: sendval, second: spillval },
            success: function (data) {
                
            }
        });
    });

});

