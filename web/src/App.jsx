import { useEffect, useMemo, useRef, useState } from 'react'
import hljs from 'highlight.js/lib/core'
import csharp from 'highlight.js/lib/languages/csharp'
import 'highlight.js/styles/github-dark.css'

hljs.registerLanguage('csharp', csharp)

const LEVELS = ['All', 'Easy', 'Medium', 'Hard']

function useHash() {
  const [hash, setHash] = useState(location.hash)
  useEffect(() => {
    const onChange = () => setHash(location.hash)
    addEventListener('hashchange', onChange)
    return () => removeEventListener('hashchange', onChange)
  }, [])
  return hash
}

export default function App() {
  const [list, setList] = useState([])
  const [error, setError] = useState(null)
  const hash = useHash()
  const route = hash.startsWith('#/q/') ? decodeURIComponent(hash.slice(4)) : hash === '#/helpers' ? 'helpers' : null

  useEffect(() => {
    fetch('/api/questions').then(r => r.json()).then(setList).catch(e => setError(String(e)))
  }, [])

  useEffect(() => { scrollTo(0, 0); document.querySelector('main')?.scrollTo(0, 0) }, [route])

  return (
    <div className={'app' + (route ? ' has-detail' : '')}>
      <aside>
        <Sidebar list={list} active={route} />
      </aside>
      <main>
        {error && <p className="error">Could not load questions: {error}</p>}
        {route === 'helpers' ? <Helpers /> : route ? <Detail key={route} id={route} list={list} /> : <Home list={list} />}
      </main>
    </div>
  )
}

function group(items) {
  const sections = new Map()
  for (const q of items) {
    if (!sections.has(q.section)) sections.set(q.section, new Map())
    const topics = sections.get(q.section)
    if (!topics.has(q.topic)) topics.set(q.topic, [])
    topics.get(q.topic).push(q)
  }
  return sections
}

function Sidebar({ list, active }) {
  const [query, setQuery] = useState('')
  const [level, setLevel] = useState('All')
  const q = query.trim().toLowerCase()
  const filtered = list.filter(x =>
    (level === 'All' || x.level === level) &&
    (!q || String(x.no) === q.replace('#', '') || `${x.title} ${x.topic} ${x.section}`.toLowerCase().includes(q)))
  const sections = useMemo(() => group(filtered), [filtered])
  const activeRef = useRef(null)
  useEffect(() => { activeRef.current?.scrollIntoView({ block: 'nearest' }) }, [active, list])

  return (
    <>
      <header className="brand">
        <a href="#/"><span className="logo">C#</span> Coding Questions</a>
        <small>{list.length} questions · DSA · Tree · Parallelism</small>
      </header>
      <div className="filters">
        <input type="search" placeholder="Search title, topic or #number…" value={query} onChange={e => setQuery(e.target.value)} />
        <div className="chips">
          {LEVELS.map(l => (
            <button key={l} className={'chip ' + l + (level === l ? ' on' : '')} onClick={() => setLevel(l)}>{l}</button>
          ))}
        </div>
      </div>
      <nav>
        {[...sections].map(([section, topics]) => (
          <section key={section}>
            <h2>{section}</h2>
            {[...topics].map(([topic, items]) => (
              <details key={topic} open={!!q || level !== 'All' || items.some(x => x.id === active)}>
                <summary>{topic}<span className="count">{items.length}</span></summary>
                <ol>
                  {items.map(x => (
                    <li key={x.id}>
                      <a href={`#/q/${x.id}`} className={x.id === active ? 'active' : ''} ref={x.id === active ? activeRef : null}>
                        <span className="no">{x.no}</span>
                        <span className="title">{x.title}</span>
                        <span className={'dot ' + x.level} title={x.level} />
                      </a>
                    </li>
                  ))}
                </ol>
              </details>
            ))}
          </section>
        ))}
        {list.length > 0 && filtered.length === 0 && <p className="muted pad">No matches.</p>}
      </nav>
      <footer className="pad muted"><a href="#/helpers">Shared helpers (Lib.cs)</a></footer>
    </>
  )
}

function Home({ list }) {
  const sections = group(list)
  return (
    <div className="page">
      <h1>C# Coding Questions</h1>
      <p className="muted">From scratch to advanced: data structures & algorithms, trees, and parallel programming. Every answer is real C# that runs on the server when you open it, so the output you see is actual output.</p>
      <div className="stats">
        {[...sections].map(([section, topics]) => (
          <div key={section} className="stat">
            <b>{[...topics.values()].reduce((n, t) => n + t.length, 0)}</b>
            <span>{section}</span>
            <small>{[...topics.keys()].join(' · ')}</small>
          </div>
        ))}
      </div>
      {list[0] && <a className="btn" href={`#/q/${list[0].id}`}>Start from #1 →</a>}
    </div>
  )
}

function Code({ source }) {
  const html = useMemo(() => hljs.highlight(source, { language: 'csharp' }).value, [source])
  return <pre className="code hljs"><code dangerouslySetInnerHTML={{ __html: html }} /></pre>
}

function Output({ result, running, onRun }) {
  return (
    <div className="card output">
      <div className="card-head">
        <h3>Output</h3>
        {result && <small className="muted">{result.ms.toFixed(1)} ms · {result.cores} cores</small>}
        <button className="btn small" onClick={onRun} disabled={running}>{running ? 'Running…' : '▶ Run again'}</button>
      </div>
      <pre className="out">
        {result
          ? result.output.trimEnd().split(/\r?\n/).map((line, i) => (
              <div key={i} className={line.startsWith('✓') ? 'ok' : line.startsWith('✗') || line.startsWith('💥') ? 'bad' : ''}>{line || ' '}</div>
            ))
          : 'Running…'}
      </pre>
    </div>
  )
}

function Detail({ id, list }) {
  const [q, setQ] = useState(null)
  const [result, setResult] = useState(null)
  const [running, setRunning] = useState(false)
  const [notFound, setNotFound] = useState(false)

  const run = () => {
    setRunning(true)
    fetch(`/api/questions/${id}/run`, { method: 'POST' })
      .then(r => r.json()).then(setResult)
      .catch(e => setResult({ output: '💥 ' + e, ms: 0, cores: 0 }))
      .finally(() => setRunning(false))
  }

  useEffect(() => {
    fetch(`/api/questions/${id}`).then(r => (r.ok ? r.json() : Promise.reject())).then(setQ).catch(() => setNotFound(true))
    run()
  }, [id])

  if (notFound) return <div className="page"><a href="#/" className="back">← All questions</a><h1>Not found</h1></div>
  if (!q) return <div className="page muted">Loading…</div>

  const prev = list[q.no - 2], next = list[q.no]
  return (
    <div className="page">
      <a href="#/" className="back">← All questions</a>
      <div className="crumbs muted">{q.section} › {q.topic}</div>
      <h1><span className="no big">#{q.no}</span> {q.title} <span className={'badge ' + q.level}>{q.level}</span></h1>
      <p className="question">{q.text}</p>
      <div className="split">
        <div className="card">
          <div className="card-head"><h3>Solution (C#)</h3><small className="muted">{q.id}.cs</small></div>
          <Code source={q.source} />
        </div>
        <Output result={result} running={running} onRun={run} />
      </div>
      <nav className="pager">
        {prev ? <a className="btn ghost" href={`#/q/${prev.id}`}>← {prev.title}</a> : <span />}
        {next && <a className="btn ghost" href={`#/q/${next.id}`}>{next.title} →</a>}
      </nav>
    </div>
  )
}

function Helpers() {
  const [src, setSrc] = useState('')
  useEffect(() => { fetch('/api/helpers').then(r => r.text()).then(setSrc) }, [])
  return (
    <div className="page">
      <a href="#/" className="back">← All questions</a>
      <h1>Shared helpers</h1>
      <p className="question">Every question uses these: <code>Check</code> prints ✓/✗ against the expected answer, <code>Fmt</code> prints arrays and lists, <code>ListNode</code> and <code>TreeNode</code> build inputs from arrays. It also has the runner that sends Console output from any thread back to your screen.</p>
      {src && <Code source={src} />}
    </div>
  )
}
