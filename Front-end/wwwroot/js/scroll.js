function enableDragScroll(containerId) {
    const container = document.getElementById(containerId);
    if (!container) return;

    let isDown = false;
    let startX;
    let scrollLeft;

    container.addEventListener('mousedown', (e) => {
        isDown = true;
        container.classList.add('active');
        startX = e.pageX - container.offsetLeft;
        scrollLeft = container.scrollLeft;
    });

    container.addEventListener('mouseleave', () => {
        isDown = false;
        container.classList.remove('active');
    });

    container.addEventListener('mouseup', () => {
        isDown = false;
        container.classList.remove('active');
    });

    container.addEventListener('mousemove', (e) => {
        if (!isDown) return;
        e.preventDefault();
        const x = e.pageX - container.offsetLeft;
        const walk = (x - startX) * 1; // Quanto mais alto o número, maior a velocidade
        container.scrollLeft = scrollLeft - walk;
    });
}

window.domHelper = {
    // Função para obter a largura de um elemento
    getElementWidth: function (element) {
        if (element) {
            return element.offsetWidth; // Retorna a largura total do elemento
        }
        return 0;
    }
};

window.resizeHelper = {
    observer: null,

    init: function (dotNetReference, elementId) {
        const element = document.getElementById(elementId);
        if (!element) return;

        // Configura o ResizeObserver para monitorar mudanças de tamanho no elemento
        this.observer = new ResizeObserver(() => {
            dotNetReference.invokeMethodAsync('UpdateScrollContainerWidth')
                .then(() => console.log('Largura atualizada com sucesso!'))
                .catch(err => console.error('Erro ao atualizar largura:', err));
        });

        this.observer.observe(element);
    },

    disconnect: function () {
        if (this.observer) {
            this.observer.disconnect();
            console.log('ResizeObserver desconectado.');
        }
    }
};
