import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function LoginPage() {
  const [name, setName] = useState("");
  const navigate = useNavigate();

  const handleLogin = async () => {
    const response = await fetch("http://localhost:5015/api/v1/auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name }),
    });

    if (response.ok) {
      const data = await response.json();
      localStorage.setItem("user", JSON.stringify(data));
      navigate("/events");
    } else {
      alert("Login failed. User not found.");
    }
  };

  return (
    <div className="flex flex-col gap-4 p-4 max-w-sm mx-auto mt-10">
      <h1 className="text-xl font-bold text-center">Login</h1>
      <input
        className="border p-2 rounded"
        placeholder="Enter your name"
        value={name}
        onChange={(e) => setName(e.target.value)}
      />
      <button
        className="bg-blue-600 text-white rounded p-2 hover:bg-blue-700"
        onClick={handleLogin}
      >
        Login
      </button>
    </div>
  );
}
