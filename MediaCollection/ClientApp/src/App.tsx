import { NavLink, Route, Routes } from 'react-router-dom';
import { isReadOnly } from './config';
import { LibraryPage } from './pages/LibraryPage';
import { DevicesPage } from './pages/DevicesPage';
import { LocationBasesPage } from './pages/LocationBasesPage';
import { RatingsPage } from './pages/RatingsPage';
import { ScanPage } from './pages/ScanPage';
import { PlaceholderPage } from './pages/PlaceholderPage';

function Nav() {
  const ro = isReadOnly();
  return (
    <header>
      <strong>Media Collection</strong>
      {ro && <span className="mc-readonly-badge">Read-only</span>}
      <nav>
        <NavLink end to="/" className={({ isActive }) => (isActive ? 'mc-active' : '')}>
          Library
        </NavLink>
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
      </nav>
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
