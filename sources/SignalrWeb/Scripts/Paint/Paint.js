$(document).ready(function () {

    // Variables :
    var connected = false;
    var color = "#000";
    var painting = false;
    var widthBrush = 5;
    var canvas = $("#canvas");
    var cursorX, cursorY;
    var groupName = $("#Id").val();
    var fromTouch = false;
    var fromClick = false;

    var context = canvas[0].getContext('2d');

    // Trait arrondi :
    context.lineJoin = 'round';
    context.lineCap = 'round';
    
    // Reference the auto-generated proxy for the hub.  
    var paintHub = $.connection.paintHub;
    
    // Create a function that the hub can call back to display messages.
    paintHub.client.AddNewLine = function (line) {
        drawline(line.x, line.y, line.nx, line.ny, line.c, line.b);
    };
    
    paintHub.client.RoomCountChanged = function (roomCount) {
        $("#users").html(roomCount);
    };

    $.connection.hub.start().done(function () {
        connected = true;
        paintHub.server.joinRoom(groupName);
        getRoomCount();
    });
    
    
    canvas.bind('touchstart', function (e) {
        if (!painting) {
            painting = true;
            fromTouch = true;
            // Coordonnées du click :
            cursorX = (e.originalEvent.touches[0].pageX - this.offsetLeft) - 10;
            cursorY = (e.originalEvent.touches[0].pageY - this.offsetTop) - 10;
        }
    });
    
    canvas.bind('touchmove', function (e) {
        e.preventDefault();
        if ((painting) && (connected) && (!fromClick) && (fromTouch)) {
            createLine(e.originalEvent.touches[0].pageX, e.originalEvent.touches[0].pageY, this.offsetLeft, this.offsetTop);
        }
    });

    canvas.bind('touchend', function (e) {
        if (fromTouch) {
            painting = false;
            fromTouch = false;
        }
    });
    

    // Click souris enfoncé sur le canvas, je dessine :
    canvas.mousedown(function (e) {
        if (!painting) {
            painting = true;
            fromClick = true;
            // Coordonnées de la souris :
            cursorX = (e.pageX - this.offsetLeft) - 10;
            cursorY = (e.pageY - this.offsetTop) - 10;
        }
    });

    // Relachement du Click sur tout le document, j'arrête de dessiner :
    $(this).mouseup(function () {
        if (fromClick) {
            painting = false;
            fromClick = false;
        }
    });

    // Mouvement de la souris sur le canvas :
    canvas.mousemove(function (e) {
        // Si je suis en train de dessiner (click souris enfoncé) :
        if ((painting) && (connected) && (fromClick) && (!fromTouch)) {
            createLine(e.pageX, e.pageY, this.offsetLeft, this.offsetTop);
        }
    });
    
    //a quick fix for the first call
    function getRoomCount() {
        paintHub.server.getRoomCount(groupName);
    }
    
    function createLine(pageX, pageY, offsetLeft, offsetTop) {
        var newX = (pageX - offsetLeft) - 10; // 10 = décalage du curseur
        var newY = (pageY - offsetTop) - 10;


        var line = { "x": cursorX, "y": cursorY, "nx": newX, "ny": newY, "b": widthBrush, "c": color, "g": groupName };
        paintHub.server.sendLine(line);

        cursorX = newX;
        cursorY = newY;
    }

    function drawline(oldX, oldY, newX, newY, lineColor, lineWidthBrush) {
        context.beginPath();
        context.moveTo(oldX, oldY);
        context.lineTo(newX, newY);
        context.strokeStyle = lineColor;
        context.lineWidth = lineWidthBrush;
        context.stroke();
    }


    // Pour chaque carré de couleur :
    $("#colors a").each(function () {
        // Je lui attribut une couleur de fond :
        $(this).css("background", $(this).attr("data-color"));

        // Et au click :
        $(this).click(function () {
            // Je change la couleur du pinceau :
            color = $(this).attr("data-color");

            // Et les classes CSS :
            $("#colors a").removeAttr("class", "");
            $(this).attr("class", "actif");

            return false;
        });
    });

    // Largeur du pinceau :
    $("#brush input").change(function () {
        if (!isNaN($(this).val())) {
            widthBrush = $(this).val();
            $("#output").html($(this).val() + " pixels");
        }
    });


    
});