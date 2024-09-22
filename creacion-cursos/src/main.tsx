// import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { BrowserRouter } from 'react-router-dom'
import Nav from './Nav.tsx'

createRoot(document.getElementById('root')!).render(
  <BrowserRouter>
    <App />
  </BrowserRouter>,
)
createRoot(document.getElementById("nav")!).render(
  <BrowserRouter>
      <Nav />
  </BrowserRouter>
);
