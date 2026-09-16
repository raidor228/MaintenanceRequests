document.addEventListener('DOMContentLoaded', initialize);

async function initialize() {
    await loadCurrentUser();
}

async function loadCurrentUser() {
    const userPanel =
        document.getElementById('userPanel');

    const actions =
        document.getElementById('actions');

    try {
        const response =
            await fetch('/api/auth/me');

        if (!response.ok) {
            window.location.href = '/login.html';
            return;
        }

        const user = await response.json();

        userPanel.innerHTML = `
            <p>
                Вы вошли как:
                <strong>${escapeHtml(user.userName)}</strong>
            </p>

            <p>
                Роль:
                <strong>${escapeHtml(user.roles.join(', '))}</strong>
            </p>

            <button id="logoutButton">
                Выйти
            </button>
        `;

        document
            .getElementById('logoutButton')
            .addEventListener('click', logout);

        if (user.roles.includes('Client')) {
            actions.innerHTML = `
                <a href="/create-request.html">
                    Создать заявку
                </a>
            `;

            await loadMyRequests();
        }

        if (user.roles.includes('Worker')) {
            await loadAssignedRequests();
        }

        if (user.roles.includes('Admin')) {
            await loadAllRequests();
        }
    }
    catch (error) {
        console.error(error);

        userPanel.textContent =
            'Не удалось получить данные пользователя.';
    }
}

async function loadMyRequests() {
    const response =
        await fetch('/api/repair-requests/my');

    if (!response.ok) {
        throw new Error(
            'Не удалось загрузить заявки клиента.'
        );
    }

    const requests = await response.json();

    displayRequests(requests);
}

async function loadAssignedRequests() {
    const response =
        await fetch('/api/repair-requests/assigned');

    if (!response.ok) {
        throw new Error(
            'Не удалось загрузить назначенные заявки.'
        );
    }

    const requests = await response.json();

    displayRequests(requests);
}

async function loadAllRequests() {
    const response =
        await fetch('/api/repair-requests');

    if (!response.ok) {
        throw new Error(
            'Не удалось загрузить заявки.'
        );
    }

    const requests = await response.json();

    displayRequests(requests);
}

function displayRequests(requests) {
    const container =
        document.getElementById('requestsContainer');

    if (requests.length === 0) {
        container.innerHTML =
            '<p>Заявок пока нет.</p>';

        return;
    }

    container.innerHTML = '';

    for (const request of requests) {
        container.appendChild(
            createRequestElement(request)
        );
    }
}

function createRequestElement(request) {
    const article =
        document.createElement('article');

    article.classList.add('request-card');

    article.innerHTML = `
        <h3>
            ${escapeHtml(request.title)}
        </h3>

        <p>
            <strong>Категория:</strong>
            ${escapeHtml(
        request.category?.name ?? 'Не указана'
    )}
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

async function logout() {
    try {
        const response =
            await fetch('/api/auth/logout', {
                method: 'POST'
            });

        if (!response.ok) {
            throw new Error(
                'Не удалось выполнить выход.'
            );
        }

        window.location.href = '/login.html';
    }
    catch (error) {
        console.error(error);

        alert('Не удалось выполнить выход.');
    }
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
    const element =
        document.createElement('div');

    element.textContent = value;

    return element.innerHTML;
}