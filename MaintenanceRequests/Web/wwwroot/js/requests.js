const form = document.getElementById('createRequestForm');
const categorySelect = document.getElementById('category');

document.addEventListener('DOMContentLoaded', loadCategories);

async function loadCategories() {
    try {
        const response = await fetch('/api/repair-categories');

        const text = await response.text();

        console.log('HTTP status:', response.status);
        console.log('Response:', text);

        if (!response.ok) {
            throw new Error(
                `HTTP ${response.status}: ${text}`
            );
        }

        const categories = JSON.parse(text);

        for (const category of categories) {
            const option = document.createElement('option');

            option.value = category.id;
            option.textContent = category.name;

            categorySelect.appendChild(option);
        }
    }
    catch (error) {
        console.error('Ошибка загрузки категорий:', error);

        showError(error.message);
    }
}

form.addEventListener('submit', createRequest);

async function createRequest(event) {
    event.preventDefault();

    const request = {
        title: document.getElementById('title').value.trim(),

        description:
            document.getElementById('description').value.trim(),

        priority:
            Number(document.getElementById('priority').value),

        categoryId:
            Number(categorySelect.value)
    };

    try {
        const response = await fetch(
            '/api/repair-requests',
            {
                method: 'POST',

                headers: {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify(request)
            }
        );

        if (!response.ok) {
            throw new Error(
                'Не удалось создать заявку.'
            );
        }

        const createdRequest = await response.json();

        console.log(
            'Заявка создана:',
            createdRequest
        );

        window.location.href = '/index.html';
    }
    catch (error) {
        console.error(error);

        showError(
            'Не удалось создать заявку.'
        );
    }
}

function showError(message) {
    alert(message);
}