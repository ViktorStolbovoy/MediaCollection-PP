import { useState } from 'react';
import { NavLink, Route, Routes, useNavigate } from 'react-router-dom';
import { setReadOnly, useIsReadOnly } from './config';
import { LoginModal } from './components/LoginModal';
import { DevicesPage } from './pages/DevicesPage';
import { LibraryPage } from './pages/LibraryPage';
import { LocationBasesPage } from './pages/LocationBasesPage';
import { PlaceholderPage } from './pages/PlaceholderPage';
import { RatingsPage } from './pages/RatingsPage';
import { ScanPage } from './pages/ScanPage';

function Nav() {
  const ro = useIsReadOnly();
  const navigate = useNavigate();
  const [showLogin, setShowLogin] = useState(false);
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
            <NavLink to="/provider" className={({ isActive }) => (isActive ? 'mc-active' : '')}>
              Provider (TMDB)
            </NavLink>
            <NavLink to="/seasons" className={({ isActive }) => (isActive ? 'mc-active' : '')}>
              Seasons tool
            </NavLink>
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
          <Route
            path="/provider"
            element={
              <PlaceholderPage
                title="Provider / TMDB lookup"
                body="The desktop “Search provider” and bulk TMDB update dialogs are not ported yet. Use the desktop app for automated metadata fetch."
              />
            }
          />
          <Route
            path="/seasons"
            element={
              <PlaceholderPage
                title="Seasons tool"
                body="Bulk season restructuring from the desktop Seasons Tool is not available in the web UI yet."
              />
            }
          />
        </Routes>
      </main>
    </div>
  );
}
