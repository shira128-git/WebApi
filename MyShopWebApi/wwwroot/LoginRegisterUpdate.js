

Register = async () => {
    const userName = document.getElementById("user_name").value;
    const password = document.getElementById("password").value;
    const firstName = document.getElementById("first_name").value;
    const lastName = document.getElementById("last_name").value;
    if (!userName || !password || !firstName || !lastName) {
        alert("All fields are required.");
        return false;
    }

    if (!userName || userName.trim().length < 3) {
        alert("User name must be at least 3 characters long.");
        return false;
    }
    if (firstName && !/^[a-zA-Zא-ת]+$/.test(firstName)) {
        alert("First name must not contain special characters.");
        return false;
    }
    if (lastName && !/^[a-zA-Zא-ת]+$/.test(lastName)) {
        alert("Last name must not contain special characters.");
        return false;
    }
    const hardPassword = await CheckPassword(password);
    if (hardPassword < 2) {
        alert("your password is low....")
        return false;
    }

    const user = {
        UserName: userName,
        Password: password,
        FirstName: firstName,
        LastName: lastName
    }

    try {
        const responsePost = await fetch("api/Users/register", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(user)
        });
        if (responsePost.ok) {
            try {
                const userDetails = await responsePost.json();
                if (userDetails && userDetails.id) {
                    localStorage.setItem("UserId", userDetails.id);
                }
            } catch (e) { }
            alert("user registered successfully");
            window.location.href = 'update.html';
            return true;
        } else {
            const errorObj = await responsePost.json();
            alert(errorObj.message || "Registration failed. Please try again.");
            return false;
        }
    }
    catch (e) {
        alert("Error: " + e.message);
        return false;
    }
}

Login = async () => {
    const userName = document.getElementById("user_name_login").value;
    const password = document.getElementById("password_login").value;

    if (!userName || !password) {
        alert("username and password are required");
        return false;
    }

    const user = {
        UserName: userName,
        Password: password,
    }

    try {
        const responsePost = await fetch("api/Users/login", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(user)
        });
        let userDatails = null;
        if (responsePost.ok) {
            userDatails = await responsePost.json();
            alert(`${userDatails.userName} loginnnnn`);
            localStorage.setItem("UserId", userDatails.id);
            window.location.href = 'update.html';
            return true;
        } else {
            switch (responsePost.status) {
                case 400:
                    const badRequestData = await responsePost.json();
                    alert(`Bad request: ${badRequestData.message || 'Invalid input. Please check your data.'}`);
                    break;
                case 401:
                    alert("Unauthorized: Please check your credentials.");
                    break;
                case 500:
                    alert("Server error. Please try again later.");
                    break;
                default:
                    alert(`Unexpected error: ${responsePost.status}`);
            }
            return false;
        }
    }
    catch (e) {
        alert("Error: " + e.message);
        return false;
    }
}

const UpDate = async () => {
    const firstname = document.getElementById("first_name_u").value;
    const lastname = document.getElementById("last_name_u").value;
    const password = document.getElementById("password_u").value;
    const username = document.getElementById("user_name_u").value;

    if (!username || !password || !firstname || !lastname) {
        alert("All fields are required.");
        return false;
    }

    if (!username || username.trim().length < 3) {
        alert("User name must be at least 3 characters long.");
        return false;
    }
    if (firstname && !/^[a-zA-Zא-ת]+$/.test(firstname)) {
        alert("First name must not contain special characters.");
        return false;
    }
    if (lastname && !/^[a-zA-Zא-ת]+$/.test(lastname)) {
        alert("Last name must not contain special characters.");
        return false;
    }
    const hardPassword = await CheckPassword(password);
    if (hardPassword < 2) {
        alert("your password is low....")
        return false;
    }


    const id = localStorage.getItem("UserId")

    const user = {
        Id: id,
        userName: username,
        password: password,
        firstName: firstname,
        lastName: lastname
    }

    const responsePost = await fetch(`api/Users/${id}`, {
        method: 'Put',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user)
    });

    if (responsePost.ok) {
        alert("updated");
    } else {
        const errorText = await responsePost.text();
        if (errorText.includes("User name already exists")) {
            alert("User name already exists. Please choose a different user name.");
        } else {
            alert(`Error: ${errorText}`);
        }
    }
}

CheckPassword = async (password) => {
    //const password = document.getElementById("password").value;
    try {
        const responsePost = await fetch("api/Users/checkPassword", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(password)
        });
        const result = await responsePost.json()
        if (responsePost.ok) {
            return result;
        }
    }
    catch (e) {
    }
}

const CheckMyPassword = async () => {
    const result = await CheckPassword();
    alert(result);
}



