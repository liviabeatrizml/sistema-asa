function formatDDI(input) {
    // Remove tudo, exceto números
    let value = input.value.replace(/\D/g, '');
    
    // Adiciona '+' automaticamente no início
    if (value.length > 0) {
        value = '+' + value;
    }
    
    input.value = value;
}

function formatDDD(input) {
    // Remove tudo, exceto números
    let value = input.value.replace(/\D/g, '');

    // Limita o DDD a 2 dígitos
    if (value.length > 2) {
        value = value.slice(0, 2);
    }

    // Adiciona os parênteses na formatação
    if (value.length === 2) {
        value = '(' + value + ')'; // Adiciona parênteses se houver 2 dígitos
    } else if (value.length === 1) {
        value = '(' + value; // Adiciona apenas o parêntese de abertura se houver 1 dígito
    }

    // Atualiza o campo de entrada com o valor formatado
    input.value = value;
}

function handleKeyDownDDD(event, input) {
    const value = input.value;

    // Se a tecla pressionada for 'Backspace' ou 'Delete'
    if (event.key === 'Backspace' || event.key === 'Delete') {
        if (value.endsWith(')')) {
            // Remove o número e o parêntese de fechamento
            input.value = value.slice(0, -2) + (value.length > 2 ? '' : ''); // Remove o número e o parêntese
            event.preventDefault(); // Impede a ação padrão do evento
        } else if (value.endsWith('(')) {
            // Se o campo termina com '(', limpa o campo
            input.value = '';
            event.preventDefault();
        }
    }
}

function formatNumero(input) {
    let value = input.value.replace(/\D/g, ''); // Remove tudo, exceto números
    if (value.length > 10) {
        value = value.slice(0, 10); // Limita o número a 10 dígitos
    }
    
    // Formata o número no formato xxxxx-xxxx
    if (value.length > 5) {
        value = value.slice(0, 5) + '-' + value.slice(5); // Hífen após os 5 primeiros dígitos
    }
    
    input.value = value; // Atualiza o campo de entrada
}