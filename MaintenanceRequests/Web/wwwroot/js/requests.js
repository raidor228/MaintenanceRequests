document.addEventListener('DOMContentLoaded', initialize);

async function initialize() {
    const path = window.location.pathname;

    if (path === '/' || path === '/index.html') {
        await loadRequests();
    }

    if (path === '/create-request.html') {
        await initializeCreateRequestForm();
    }
}

async function loadRequests() {
    const container = document.getElementById('requestsContainer');

    try {
        const response = await fetch('/api/repair-requests');

        if (!response.ok) {
            throw new Error(
                `HTTP ${response.status}: ${await response.text()}`
            );
        }

        const requests = await response.json();

        if (requests.length === 0) {
            container.innerHTML = '<p>Заявок пока нет.</p>';
            return;
        }

        container.innerHTML = '';

        for (const request of requests) {
            const element = createRequestElement(request);

            container.appendChild(element);
        }
    }
    catch (error) {
        console.error(error);

        container.innerHTML =
            '<p>Не удалось загрузить заявки.</p>';
    }
}

function createRequestElement(request) {
    const article = document.createElement('article');

    article.classList.add('request-card');

    article.innerHTML = `
        <h3>${escapeHtml(request.title)}</h3>

        <p>
            <strong>Категория:</strong>
            ${escapeHtml(request.category?.name ?? 'Не указана')}
        </p>

        <p>
            <strong>Приоритет:</strong>
            ${getPriorityName(request.priority)}
        </p>

        <p>
            <strong>Статус:</strong>
            ${getStatusName(request.status)}
        </p>

        <p>
            <strong>Создана:</strong>
            ${formatDate(request.createdAt)}
        </p>

        <a href="/request.html?id=${request.id}">
            Открыть заявку
        </a>
    `;

    return article;
}

function getPriorityName(priority) {
    const priorities = {
        0: 'Низкий',
        1: 'Обычный',
        2: 'Высокий',
        3: 'Критический'
    };

    return priorities[priority] ?? 'Неизвестно';
}

function getStatusName(status) {
    const statuses = {
        0: 'Новая',
        1: 'На рассмотрении',
        2: 'Одобрена',
        3: 'Назначена',
        4: 'В работе',
        5: 'Ожидание',
        6: 'Завершена',
        7: 'Закрыта',
        8: 'Отклонена',
        9: 'Отменена'
    };

    return statuses[status] ?? 'Неизвестно';
}

function formatDate(date) {
    return new Date(date).toLocaleString('ru-RU');
}

function escapeHtml(value) {
    const element = document.createElement('div');

    element.textContent = value;

    return element.innerHTML;
}