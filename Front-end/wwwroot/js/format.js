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

function formatTelefone(input) {
    let value = input.value.replace(/\D/g, ''); // Remove todos os caracteres não numéricos
    
    // Limita o valor para DDI (2 dígitos), DDD (2 dígitos), e o número completo (9 dígitos)
    if (value.length > 13) {
        value = value.slice(0, 13); // Limita o valor total a 13 caracteres numéricos
    }
    
    let formattedValue = '';

    // Adiciona o DDI
    if (value.length > 0) {
        formattedValue += '+' + value.slice(0, 2); // Adiciona o DDI (2 dígitos)
    }

    // Adiciona o DDD
    if (value.length > 2) {
        formattedValue += ' (' + value.slice(2, 4) + ')'; // Adiciona o DDD (2 dígitos)
    }

    // Adiciona o número principal
    if (value.length > 4) {
        formattedValue += ' ' + value.slice(4, 9); // Primeira parte do número (5 dígitos)
    }

    // Adiciona o hífen e a segunda parte do número
    if (value.length > 9) {
        formattedValue += '-' + value.slice(9, 13); // Segunda parte do número (4 dígitos)
    }

    // Atualiza o valor do campo com a formatação correta
    input.value = formattedValue;
}

function handleKeyDown(event, input) {
    const value = input.value;

    // Se a tecla pressionada for 'Backspace' ou 'Delete'
    if (event.key === 'Backspace' || event.key === 'Delete') {
        // Obtém a posição do cursor no campo
        const cursorPosition = input.selectionStart;

        // Se a posição do cursor estiver logo após o ")", permite a exclusão correta
        if (cursorPosition > 0 && value[cursorPosition - 1] === ')') {
            // Remove o número anterior e o parêntese
            input.value = value.slice(0, cursorPosition - 2) + value.slice(cursorPosition);
            event.preventDefault(); // Impede a ação padrão de exclusão
        }
    }
}