import { useEffect, useState } from "react";

interface Event {
  id: string;
  title: string;
  start: string;
  end: string;
}

interface Participant {
  id: string;
  userId: string;
  eventId: string;
}

interface User {
  id: string;
  name: string;
}

export default function DashboardPage() {
  const [tab, setTab] = useState("events");
  const [events, setEvents] = useState<Event[]>([]);
  const [selectedEvent, setSelectedEvent] = useState<Event | null>(null);
  const [participants, setParticipants] = useState<User[]>([]);
  const [newEventTitle, setNewEventTitle] = useState("");
  const [newParticipantName, setNewParticipantName] = useState("");

  useEffect(() => {
    fetch("http://localhost:5015/api/v1/events")
      .then((res) => res.json())
      .then((data) => {
        const list = data?.$values || [];
        setEvents(list);
      });
  }, []);

  const handleAddEvent = async () => {
    const response = await fetch("http://localhost:5015/api/v1/events", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ title: newEventTitle }),
    });
    if (response.ok) {
      const event = await response.json();
      setEvents([...events, event]);
      setNewEventTitle("");
    }
  };

  const handleLoadParticipants = async (eventId: string) => {
    try {
      const partRes = await fetch(
        `http://localhost:5015/api/v1/events/${eventId}/participants`
      );
      const partData = await partRes.json();
      const rawParticipants: Participant[] = partData?.$values || [];

      const usersRes = await fetch("http://localhost:5015/api/v1/users");
      const usersData = await usersRes.json();
      const allUsers: User[] = usersData?.$values || [];

      const participantUsers = rawParticipants
        .map((p) => allUsers.find((u) => u.id === p.userId))
        .filter((u): u is User => u !== undefined);

      setParticipants(participantUsers);
      setSelectedEvent(events.find((e) => e.id === eventId) || null);
      setTab("participants");
    } catch (err) {
      console.error("Error loading participants", err);
    }
  };

  const handleRemoveParticipant = async (userId: string) => {
    if (!selectedEvent) return;
    await fetch(
      `http://localhost:5015/api/v1/events/${selectedEvent.id}/participants/${userId}`,
      {
        method: "DELETE",
      }
    );
    handleLoadParticipants(selectedEvent.id);
  };

const handleAddParticipant = async () => {
  if (!newParticipantName.trim()) return;

  try {
    // 1. Получаем список пользователей по имени
    const usersResponse = await fetch(`http://localhost:5015/api/v1/users?name=${encodeURIComponent(newParticipantName)}`);
    const users = await usersResponse.json();

    if (!Array.isArray(users) || users.length === 0) {
      alert("User not found");
      return;
    }

    const user = users[0]; // берем первого найденного
    const userId = user.id;

    // 2. Создаем участника
    const response = await fetch(`http://localhost:5015/api/v1/events/${eventId}/participants`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        userId: userId
      }),
    });

    if (response.ok) {
      alert("Participant added!");
      setNewParticipantName("");
      await loadParticipants(); // перезагрузи список
    } else {
      const text = await response.text();
      console.error("Add participant failed:", text);
      alert("Failed to add participant");
    }
  } catch (err) {
    console.error("Unexpected error:", err);
    alert("Unexpected error");
  }
};


  return (
    <div style={{ maxWidth: "900px", margin: "0 auto", padding: "20px" }}>
      <h1 style={{ textAlign: "center", marginBottom: "20px" }}>Calendar Dashboard</h1>

      <div style={{ display: "flex", gap: "10px", marginBottom: "20px" }}>
        <button onClick={() => setTab("events")}>Events</button>
        <button onClick={() => setTab("participants")} disabled={!selectedEvent}>
          Participants
        </button>
        <button onClick={() => setTab("availability")} disabled={!selectedEvent}>
          Availability
        </button>
      </div>

      {tab === "events" && (
        <div>
          <h2>All Events</h2>
          <ul>
            {events.map((e) => (
              <li key={e.id} style={{ marginBottom: "8px" }}>
                {e.title} - {e.start} to {e.end}
                <button
                  onClick={() => handleLoadParticipants(e.id)}
                  style={{ marginLeft: "10px" }}
                >
                  Manage Participants
                </button>
              </li>
            ))}
          </ul>
          <div style={{ marginTop: "20px" }}>
            <input
              placeholder="New Event Title"
              value={newEventTitle}
              onChange={(e) => setNewEventTitle(e.target.value)}
            />
            <button onClick={handleAddEvent} style={{ marginLeft: "10px" }}>
              Add
            </button>
          </div>
        </div>
      )}

      {tab === "participants" && selectedEvent && (
        <div>
          <h2>Participants for {selectedEvent.title}</h2>
          <ul>
            {participants.map((p) => (
              <li key={p.id} style={{ marginBottom: "6px" }}>
                {p.name}
                <button
                  style={{
                    marginLeft: "10px",
                    backgroundColor: "red",
                    color: "white",
                    border: "none",
                    borderRadius: "4px",
                    cursor: "pointer",
                    padding: "2px 8px",
                  }}
                  onClick={() => handleRemoveParticipant(p.id)}
                >
                  Remove
                </button>
              </li>
            ))}
          </ul>

          <div style={{ marginTop: "20px" }}>
            <h3>Add Participant by Name</h3>
            <input
              placeholder="Enter user name"
              value={newParticipantName}
              onChange={(e) => setNewParticipantName(e.target.value)}
            />
            <button
              style={{ marginLeft: "10px" }}
              onClick={handleAddParticipant}
            >
              Add
            </button>
          </div>
        </div>
      )}

      {tab === "availability" && selectedEvent && (
        <div>
          <h2>Available Time Slots for {selectedEvent.title}</h2>
          <button
            onClick={async () => {
              const res = await fetch(
                `http://localhost:5015/api/v1/events/${selectedEvent.id}/availability`
              );
              const data = await res.json();
              alert("Available: " + JSON.stringify(data));
            }}
          >
            Find Time Slot
          </button>
        </div>
      )}
    </div>
  );
}
