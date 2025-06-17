import "./../styles/LoginPage.css";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function LoginPage() {
  const [login, setLogin] = useState("");
  const [newLogin, setNewLogin] = useState("");
  const [newName, setNewName] = useState("");
  const navigate = useNavigate();

  const handleLogin = async () => {
    const response = await fetch("http://localhost:5015/api/v1/auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ login }),
    });

    if (response.ok) {
      const data = await response.json();
      localStorage.setItem("user", JSON.stringify(data));
      navigate("/dashboard");
    } else {
      alert("Login failed. User not found.");
    }
  };

  const handleRegister = async () => {
    if (!newLogin.trim() || !newName.trim()) {
      alert("Please enter both login and name");
      return;
    }

    const response = await fetch("http://localhost:5015/api/v1/users", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        login: newLogin,
        name: newName,
      }),
    });

    if (response.ok) {
      alert("User created. You can now log in.");
      setNewLogin("");
      setNewName("");
    } else {
      alert("Failed to create user. Make sure login is unique.");
    }
  };

  return (
    <div className="login-wrapper">
      <div className="login-card">
        <h1 className="login-title">Welcome to DataArt Calendar</h1>

        <input
          className="login-input"
          placeholder="Enter your login name"
          value={login}
          onChange={(e) => setLogin(e.target.value)}
        />
        <button className="login-button" onClick={handleLogin}>
          Log in
        </button>

        <hr style={{ margin: "20px 0", width: "100%" }} />

        <input
          className="login-input"
          placeholder="New login"
          value={newLogin}
          onChange={(e) => setNewLogin(e.target.value)}
        />
        <input
          className="login-input"
          placeholder="New user name"
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
        />
        <button className="login-button" onClick={handleRegister}>
          Create User
        </button>
      </div>
    </div>
  );
}
