# 🗓️ Calendar Scheduler App

A full-featured web application for managing events, participants, and finding shared available time slots. Built with ASP.NET Core WebAPI (.NET 9) and React.

---

## 🔧 Features

- 📅 CRUD operations for **Events**
- 👥 Add / Remove **Participants**
- ⏰ Find common available time slots for all participants
- 🔐 Basic authentication (WIP)
- 🧠 Bonus: LLM Integration (e.g., GitHub Copilot-like prompts)

---

## 🛠️ Tech Stack

### Backend — ASP.NET Core (.NET 9)

- WebAPI
- Entity Framework Core + SQLite (in-memory or file)
- Dependency Injection, Logging, Exception Handling
- RESTful routes: `/api/v1/events`, `/users`, `/participants`, `/availability`

### Frontend — React + Vite

- React Hooks + TypeScript
- API calls using `fetch`
- Simple layout (no UI libraries)

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourname/calendar-app.git
cd calendar-app
