import { useState } from 'react';
import { NavLink, Route, Routes, useNavigate } from 'react-router-dom';
import { LoginModal } from './components/LoginModal';
import { UpdateFromProviderModal } from './components/UpdateFromProviderModal';
import { setReadOnly, useIsReadOnly } from './config';
import { DevicesPage } from './pages/DevicesPage';
import { LibraryPage } from './pages/LibraryPage';
import { LocationBasesPage } from './pages/LocationBasesPage';
import { RatingsPage } from './pages/RatingsPage';
import { ScanPage } from './pages/ScanPage';

function Nav() {
  const ro = useIsReadOnly();
  const navigate = useNavigate();
  const [showLogin, setShowLogin] = useState(false);
  const [showProviderTool, setShowProviderTool] = useState(false);
  const [loggingOut, setLoggingOut] = useState(false);

  const enterConfiguration = () => {
    setShowLogin(true);
  };

  const onLoginSuccess = () => {
    setShowLogin(false);
    setReadOnly(false);
  };

  const exitConfiguration = async () => {
    if (loggingOut) return;
    setLoggingOut(true);
    try {
      await fetch('/api/auth/logout', {
        method: 'POST',
        credentials: 'same-origin',
      });
    } catch {
      /* best-effort: still drop UI state */
    } finally {
      setReadOnly(true);
      setLoggingOut(false);
      setShowProviderTool(false);
      navigate('/');
    }
  };

  return (
    <header>
      <strong>Media Collection</strong>
      <span className="mc-readonly-badge">{ro ? "View" : "Edit"}</span>
      <nav>
        <NavLink end to="/" className={({ isActive }) => (isActive ? 'mc-active' : '')}>
          Library
        </NavLink>
        {!ro && (
          <>
            <NavLink to="/devices" className={({ isActive }) => (isActive ? 'mc-active' : '')}>
              Devices
            </NavLink>
            <NavLink to="/locations" className={({ isActive }) => (isActive ? 'mc-active' : '')}>
              Location bases
            </NavLink>
            <NavLink to="/ratings" className={({ isActive }) => (isActive ? 'mc-active' : '')}>
              Ratings
            </NavLink>
            <NavLink to="/scan" className={({ isActive }) => (isActive ? 'mc-active' : '')}>
              Bulk scan
            </NavLink>
            <button
              type="button"
              className="mc-nav-btn"
              onClick={() => setShowProviderTool(true)}
            >
              Update from TMDB
            </button>
          </>
        )}
        {ro ? (
          <button type="button" className="mc-config-btn" onClick={enterConfiguration}>
            Configure
          </button>
        ) : (
          <button type="button" className="mc-config-btn" onClick={exitConfiguration} disabled={loggingOut}>
            {loggingOut ? 'Signing out…' : 'Exit Configuration'}
          </button>
        )}
      </nav>
      {showLogin && (
        <LoginModal
          onCancel={() => setShowLogin(false)}
          onSuccess={onLoginSuccess}
        />
      )}
      {showProviderTool && !ro && (
        <UpdateFromProviderModal onClose={() => setShowProviderTool(false)} />
      )}
    </header>
  );
}

export function App() {
  return (
    <div className="mc-app">
      <Nav />
      <main className="mc-main">
        <Routes>
          <Route path="/" element={<LibraryPage />} />
          <Route path="/devices" element={<DevicesPage />} />
          <Route path="/locations" element={<LocationBasesPage />} />
          <Route path="/ratings" element={<RatingsPage />} />
          <Route path="/scan" element={<ScanPage />} />
        </Routes>
      </main>
    </div>
  );
}
