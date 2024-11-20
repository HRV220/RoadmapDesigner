import React from "react";

const LoginButton = () => {
    const handleLogin = () => {
        window.location.href = "https://localhost:7244/login";
    };

    return <button onClick={handleLogin}>Login with GitHub</button>;
};

export default LoginButton;
