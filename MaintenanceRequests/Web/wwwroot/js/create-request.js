document.addEventListener(
    'DOMContentLoaded',
    initialize
);

async function initialize() {
    const user = await loadCurrentUser();

    if (!user) {
        return;
    }

    if (!user.roles.includes('Client')) {
        showError(
            'Только клиент может создавать заявки.'
        );

        return;
    }

    await loadCategories();

    const form =
        document.getElementById(
            'createRequestForm'
        );

    form.addEventListener(
        'submit',
        createRequest
    );
}

async function loadCurrentUser() {
    try {
        const response =
            await fetch('/api/auth/me');

        if (!response.ok) {
            window.location.href =
                '/login.html';

            return null;
        }

        return await response.json();
    }
    catch (error) {
        console.error(error);

        showError(
            'Не удалось проверить авторизацию.'
        );

        return null;
    }
}

async function loadCategories() {
    const categorySelect =
        document.getElementById('category');

    try {
        const response =
            await fetch('/api/repair-categories');

        if (!response.ok) {
            throw new Error(
                'Не удалось загрузить категории.'
            );
        }

        const categories =
            await response.json();

        for (const category of categories) {
            const option =
                document.createElement('option');

            option.value = category.id;

            option.textContent =
                category.name;

            categorySelect.appendChild(
                option
            );
        }
    }
    catch (error) {
        console.error(error);

        showError(error.message);
    }
}

async function createRequest(event) {
    event.preventDefault();

    clearError();

    const title =
        document.getElementById('title')
            .value
            .trim();

    const description =
        document.getElementById('description')
            .value
            .trim();

    const priority =
        Number(
            document.getElementById('priority')
                .value
        );

    const categoryId =
        Number(
            document.getElementById('category')
                .value
        );

    if (!categoryId) {
        showError(
            'Выберите категорию.'
        );

        return;
    }

    try {
        const response =
            await fetch(
                '/api/repair-requests',
                {
                    method: 'POST',

                    headers: {
                        'Content-Type':
                            'application/json'
                    },

                    body: JSON.stringify({
                        title,
                        description,
                        priority,
                        categoryId
                    })
                }
            );

        const text =
            await response.text();

        if (!response.ok) {
            let message =
                'Не удалось создать заявку.';

            try {
                const error =
                    JSON.parse(text);

                if (error.error) {
                    message = error.error;
                }
            }
            catch {
                // Ответ не является JSON.
            }

            throw new Error(message);
        }

        const request =
            JSON.parse(text);

        window.location.href =
            `/request.html?id=${request.id}`;
    }
    catch (error) {
        console.error(error);

        showError(error.message);
    }
}

function showError(message) {
    const errorMessage =
        document.getElementById(
            'errorMessage'
        );

    errorMessage.textContent = message;
}

function clearError() {
    const errorMessage =
        document.getElementById(
            'errorMessage'
        );

    errorMessage.textContent = '';
}