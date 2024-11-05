import { useEffect, useState } from 'react';

function UserList() {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        populateUserData();
    }, []);

    async function populateUserData() {
        try {
            const response = await fetch('https://localhost:7244/Roadmap');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            setUsers(data);
        } catch (error) {
            console.error("Ошибка загрузки данных пользователей:", error);
        } finally {
            setLoading(false);
        }
    }

    const contents = loading ? (
        <p><em>Loading... Please refresh the page if it takes too long.</em></p>
    ) : (
        <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>User ID</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Email</th>
                </tr>
            </thead>
            <tbody>
                {users.map(user => (
                    <tr key={user.userID}>
                        <td>{user.userID}</td>
                        <td>{user.firstName}</td>
                        <td>{user.lastName}</td>
                        <td>{user.email}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );

    return (
        <div>
            <h1 id="tableLabel">User List</h1>
            <p>This component demonstrates fetching user data from the server.</p>
            {contents}
        </div>
    );
}

export default UserList;
