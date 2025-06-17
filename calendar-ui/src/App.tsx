import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import EventsPage from "./pages/EventsPage"; // создадим позже

function App() {
  const user = JSON.parse(localStorage.getItem("user") || "null");

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route
          path="/events"
          element={user ? <EventsPage /> : <Navigate to="/login" replace />}
        />
        <Route path="*" element={<Navigate to={user ? "/events" : "/login"} />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
