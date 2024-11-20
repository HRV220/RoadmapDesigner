import React, { useEffect } from "react";
import axios from "axios";

const CallbackPage = () => {
    useEffect(() => {
        const fetchUser = async () => {
            const code = new URLSearchParams(window.location.search).get("code");
            if (code) {
                const response = await axios.get(`http://localhost:7244/callback?code=${code}`);
                console.log(response.data);
                // Сохраните данные пользователя или токен
            }
        };
        fetchUser();
    }, []);

    return <div>Loading...</div>;
};

export default CallbackPage;
