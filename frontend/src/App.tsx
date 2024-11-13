// App.tsx

import React, { useState, useEffect } from 'react';

const App: React.FC = () => {
  // State to store the response from the Go backend
  const [message, setMessage] = useState<string>('');

  // Fetch the data from the Go backend when the component mounts
  useEffect(() => {
    // Make a GET request to your Go server
    fetch('http://localhost:8080/')
      .then((response) => response.text()) // Convert response to text
      .then((data) => setMessage(data))    // Set the fetched message to state
      .catch((error) => console.error('Error fetching data:', error)); // Handle errors
  }, []);

  return (
    <div className="App">
      <h1>Message from Go Backend:</h1>
      <p>{message}</p>
    </div>
  );
};

export default App;
