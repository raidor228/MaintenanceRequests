document.addEventListener('DOMContentLoaded', loadRequest);

async function loadRequest() {
    const container = document.getElementById('requestContainer');

    const parameters = new URLSearchParams(window.location.search);
    const id = parameters.get('id');

    if (!id) {
        container.innerHTML =
            '<p>Не указан идентификатор заявки.</p>';

        return;
    }

    try {
        const response = await fetch(
            `/api/repair-requests/${id}`
        );

        if (!response.ok) {
            if (response.status === 404) {
                throw new Error('Заявка не найдена.');
            }

            throw new Error(
                `HTTP ${response.status}: ${await response.text()}`
            );
        }

        const request = await response.json();

        displayRequest(request);
    }
    catch (error) {
        console.error(error);

        container.innerHTML = `
            <p>${escapeHtml(error.message)}</p>
        `;
    }
}

function displayRequest(request) {
    const container = document.getElementById('requestContainer');

    container.innerHTML = `
        <h1>
            ${escapeHtml(request.title)}
        </h1>

        <section>
            <p>
                <strong>ID:</strong>
                ${request.id}
            </p>

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
                <strong>Дата создания:</strong>
                ${formatDate(request.createdAt)}
            </p>

            <p>
                <strong>Описание:</strong>
            </p>

            <p>
                ${escapeHtml(request.description)}
            </p>
        </section>

        <hr>

        <section>
            <h2>История статусов</h2>

            <div id="statusHistory">
                ${createStatusHistory(request.statusHistory)}
            </div>
        </section>

        <hr>

        <section>
            <h2>Комментарии</h2>

            <div id="comments">
                ${createComments(request.comments)}
            </div>
        </section>
    `;
}

function createStatusHistory(history) {
    if (!history || history.length === 0) {
        return '<p>История изменений отсутствует.</p>';
    }

    return history
        .map(item => `
            <article>
                <p>
                    <strong>
                        ${getStatusName(item.oldStatus)}
                        →
                        ${getStatusName(item.newStatus)}
                    </strong>
                </p>

                <p>
                    ${formatDate(item.changedAt)}
                </p>

                ${
            item.comment
                ? `<p>${escapeHtml(item.comment)}</p>`
                : ''
        }
            </article>
        `)
        .join('');
}

function createComments(comments) {
    if (!comments || comments.length === 0) {
        return '<p>Комментариев пока нет.</p>';
    }

    return comments
        .map(comment => `
            <article>
                <p>
                    ${escapeHtml(comment.text)}
                </p>

                <small>
                    ${formatDate(comment.createdAt)}
                </small>
            </article>
        `)
        .join('');
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