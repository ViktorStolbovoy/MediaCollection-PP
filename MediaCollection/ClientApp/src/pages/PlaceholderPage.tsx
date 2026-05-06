export function PlaceholderPage({ title, body }: { title: string; body: string }) {
  return (
    <div>
      <h2>{title}</h2>
      <p className="mc-muted">{body}</p>
    </div>
  );
}
