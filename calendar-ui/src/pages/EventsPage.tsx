export default function EventsPage() {
  const user = JSON.parse(localStorage.getItem("user") || "null");

  return (
    <div className="p-6">
      <h1 className="text-xl font-bold">Welcome, {user?.name}</h1>
      <p>Your user ID: {user?.id}</p>
    </div>
  );
}
