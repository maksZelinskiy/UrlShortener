import {inputTypes} from "./text";

const emailPattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
const lowerCaseLetters = /[a-z]/g;
const upperCaseLetters = /[A-Z]/g;

export const validateInput = (input, updates) => {
    const value = input.value;
    const response = {isValid: false, message: "", setEvent: null}

    switch (input.name) {
        case inputTypes.email:
            response.setEvent = updates.setEmail;

            if (!value.toLowerCase().match(emailPattern)) {
                response.message = "Email is incorrect";
                response.isValid = false;
                return response;
            }
            response.message = '';
            response.isValid = true;
            return response;
        case inputTypes.password:
            response.setEvent = updates.setPassword;
            response.isValid = false;
            if (!value.match(lowerCaseLetters)) {
                response.message = "Must be at least one lower case letter";
                return response;
            }
            if (!value.match(upperCaseLetters)) {
                response.message = "Must be at least one upper case letter";
                return response;
            }
            if (value.length < 6) {
                response.message = "Min length is 6 characters";
                return response;
            }

            response.message = '';
            response.isValid = true;
            return response;
        case inputTypes.firstName:
            response.setEvent = updates.setFirstName;
            if (value.length < 3) {
                response.message = "Enter at least 3 characters";
                response.isValid = false;
                return response;
            }

            response.message = '';
            response.isValid = true;
            return response;
        case inputTypes.lastName:
            response.setEvent = updates.setLastName;
            if (value.length < 3) {
                response.message = "Enter at least 3 characters";
                response.isValid = false;
                return response;
            }

            response.message = '';
            response.isValid = true;
            return response;
        default:
            return response;
    }
}

export const validateFrom = (form, updates) => {
    const formResponse = {isValid: true, data: {}}

    for (const element of form.elements) {
        if (element.tagName !== "INPUT")
            continue;

        const response = validateInput(element, updates);

        if (!response.isValid) {
            response.setEvent(response.message);
            element.addEventListener("input", (event) => setEvent(event, response.setEvent));
            formResponse.isValid = false;
        } else
            formResponse.data[element.name] = element.value;
    }

    return formResponse;
}

export const setEvent = (event, setError) => {
    const element = event.target;
    const response = validateInput(element, {});

    setError(response.message);
}