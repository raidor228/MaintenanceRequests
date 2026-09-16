document.addEventListener('DOMContentLoaded', initialize);

async function initialize() {
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');

    if (loginForm) {
        loginForm.addEventListener('submit', login);
    }

    if (registerForm) {
        registerForm.addEventListener('submit', register);
    }
}

async function login(event) {
    event.preventDefault();

    const userName =
        document.getElementById('userName').value.trim();

    const password =
        document.getElementById('password').value;

    clearError();

    try {
        const response = await fetch('/api/auth/login', {
            method: 'POST',

            headers: {
                'Content-Type': 'application/json'
            },

            body: JSON.stringify({
                userName,
                password
            })
        });

        if (!response.ok) {
            const error = await response.json();

            throw new Error(
                error.error ??
                'Не удалось выполнить вход.'
            );
        }

        window.location.href = '/index.html';
    }
    catch (error) {
        console.error(error);

        showError(error.message);
    }
}

async function register(event) {
    event.preventDefault();

    const userName =
        document.getElementById('userName').value.trim();

    const email =
        document.getElementById('email').value.trim();

    const password =
        document.getElementById('password').value;

    const confirmPassword =
        document.getElementById('confirmPassword').value;

    clearError();

    if (password !== confirmPassword) {
        showError('Пароли не совпадают.');
        return;
    }

    try {
        const response = await fetch('/api/auth/register', {
            method: 'POST',

            headers: {
                'Content-Type': 'application/json'
            },

            body: JSON.stringify({
                userName,
                email,
                password
            })
        });

        const text = await response.text();

        if (!response.ok) {
            let message = 'Не удалось зарегистрироваться.';

            try {
                const errors = JSON.parse(text);

                if (Array.isArray(errors)) {
                    message = errors.join('\n');
                }
                else if (errors.error) {
                    message = errors.error;
                }
            }
            catch {
                // Ответ не является JSON.
            }

            throw new Error(message);
        }

        window.location.href = '/login.html';
    }
    catch (error) {
        console.error(error);

        showError(error.message);
    }
}

function showError(message) {
    const errorMessage =
        document.getElementById('errorMessage');

    errorMessage.textContent = message;
}

function clearError() {
    const errorMessage =
        document.getElementById('errorMessage');

    errorMessage.textContent = '';
}