var pedidoIdGlobal;
function ModalCancelacion(pedidoId) {
    pedidoIdGlobal = pedidoId;
    $("#CancelarPedido").modal();
}

function Confirmar(estado) {
    $.ajax({
        type: "Post",
        url: "../Pedidos/ActualizarEstadoPedido",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ pedidoId: pedidoIdGlobal, estado: estado }),
        success: function (data) {
            $("#CancelarPedido").modal('hide');
            $("#OpcionesPedido").modal('hide');
            toastr.success("Pedido " + estado+" Correctamente");
            $("#" + pedidoIdGlobal).html(estado);
        },
        error: function (data) {
            toastr.error("Error al Cancelar pedido");
        }
    });
}

function ModalOpciones(pedidoId) {
    pedidoIdGlobal = pedidoId;
    $("#OpcionesPedido").modal();
}