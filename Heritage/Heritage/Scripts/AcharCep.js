jQuery(function () {
   
    $('#ceperrado').hide();
    $('#loading').hide();
    $('#buscar-cep').click(function () {
        $('#cep').cep({
            load: function () {
                //Exibe a div loading
                $('#loading').show();
                $('#ceperrado').hide();
            },
            complete: function () {
                //Esconde a div loading
                $('#loading').hide();
            },
            error: function (msg) {
                //Exibe em alert a mensagem de erro retornada
                //alert(msg);
                $('#ceperrado').show();
                $('#loading').hide();
            },
            success: function (data) {
                $('#ceperrado').hide();
                $('#loading').hide();
                $('#logradouro').val(data.tipoLogradouro + ' ' + data.logradouro);
                $('#bairro').val(data.bairro);
                $('#estado').val(data.estado);
                $('#cidade').val(data.cidade);

            }
        });
    });

});