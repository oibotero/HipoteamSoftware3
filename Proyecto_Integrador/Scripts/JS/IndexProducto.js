var globalId;
var ListaId = [];
function prueba(id) {
    globalId = id;
    $("#mi-modal").modal('show');
}

function Si() {
    var cantidad = $("#Cantidad").val();
    if (cantidad !== "") {
        for (var i = 0; i < cantidad; i++) {
            ListaId[ListaId.length] = globalId;
        }
    } else {
        ListaId[ListaId.length] = globalId;
    }
    
    toastr.success('Añadido correctamente!!!');
    $("#mi-modal").modal('hide');
}

function No() {
    $("#mi-modal").modal('hide');
    
}
function Finalizar() {
    if (ListaId.length === 0) {
        toastr.error('No hay Productos seleccionados!!!');
    } else {
        $("#Finalizar").modal('show');
    }
    
    
}

function guardarPedido() {
    var direccion = $("#direccion").val();
    if (ListaId.length === 0) {
        toastr.error('No hay Productos');
    } else {

        $.ajax({
            type: "Post",
            url: "../Pedidos/Insert",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ productos: ListaId, direccion: direccion }),
            success: function (data) {
                $("#Finalizar").modal('hide');
                window.location.href = "../Pedidos/listaPedidos";
            },
            error: function (data) {
                toastr.error(data);
            }
        });
    }

}
function Cancelar() {
    $("#Finalizar").modal('hide');
    ListaId = [];
}

$(document).ready(function () {

    $('[data-toggle="tooltip"]').tooltip();

});

$(function () {
    $('.material-tooltip-main').tooltip({
        template: '<div class="tooltip md-tooltip"><div class="tooltip-arrow md-arrow"></div><div class="tooltip-inner md-inner"></div></div>'
    });
})
