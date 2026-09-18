export default function LevelBadge({ level }) {
  return <span className={`level-badge ${level.toLowerCase()}`}>{level}</span>
}
