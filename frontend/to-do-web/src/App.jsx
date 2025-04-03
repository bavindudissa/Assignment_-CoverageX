import React, { useEffect, useState } from "react";
import TaskList from "./components/TaskList";
import TaskForm from "./components/TaskForm";

function App() {
  const [refresh, setRefresh] = useState(false);

  const handleTaskAdded = () => {
    setRefresh(!refresh);
  };

  return (
    <div className="app-container">
    <h1>To-Do App</h1>
    <div className="main-content">
      <div className="form-container">
        <TaskForm onTaskAdded={handleTaskAdded} />
      </div>
      <div className="task-list-container">
        <TaskList key={refresh} />
      </div>
    </div>
  </div>
  )
}

export default App
