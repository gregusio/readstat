import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import Home from './pages/Home';

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path='/register' element={<Register />} />
        <Route path="/login" element={<Login />} />
        <Route path="/home" element={<Home />} />
        <Route path="*" element={<Login />} />
      </Routes>
    </Router>
  );
};

export default App;
