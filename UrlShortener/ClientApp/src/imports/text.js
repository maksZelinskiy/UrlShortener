export const inputTypes = {
    firstName: "firstName",
    lastName: "lastName",
    password: "password",
    email: "email",
}

export const messageTypes = {
    error: "error",
    warning: "warn",
    success: "success",
    info: "info"
}

export const logout = () => {
    localStorage.removeItem('bearer_token');
}

export const formatDate = (date) => {
    if (!date)
        return 'No Date';

    const splits = date.split('T');

    return splits[0] + ' ' + formatTime(splits[1]);
}

export const formatTime = (date) => {
    if (!date)
        return '';

    return date.split('.')[0];
}

export const getFormData = (form, test) => {
    const data = {};

    for (const element of form.elements) {
        if (element.tagName === "BUTTON" || element.name === "")
            continue;
        if ((test && element.checked) || !test) {
            data[element.name] = element.value;
        }
    }

    return data;
}

export const capitalize = (str) => str.charAt(0).toUpperCase() + str.slice(1);
