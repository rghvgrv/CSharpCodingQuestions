import { useEffect, useState } from 'react'
import { getJson } from '../api.js'
import Markdown from './Markdown.jsx'
import CodeBlock from './CodeBlock.jsx'
import LevelBadge from './LevelBadge.jsx'

export default function QuestionPage({ questionId, allQuestions }) {
  const [question, setQuestion] = useState(null)
  const [error, setError] = useState(null)

  useEffect(() => {
    getJson(`/api/questions/${questionId}`).then(setQuestion).catch(e => setError(e.message))
  }, [questionId])

  if (error) return <div className="page error">Question not found.</div>
  if (!question) return <div className="page muted">Loading…</div>

  const index = allQuestions.findIndex(q => q.id === question.id)
  const previous = allQuestions[index - 1]
  const next = allQuestions[index + 1]
  const hasComplexity = question.approaches.some(a => a.time)

  return (
    <article className="page">
      <a href={`#/topic/${question.topic.id}`} className="back">← {question.topic.title}</a>
      <div className="crumbs">
        {question.topic.sectionTitle} › <a href={`#/topic/${question.topic.id}`}>{question.topic.title}</a>
      </div>
      <h1><span className="number">#{question.number}</span> {question.title} <LevelBadge level={question.level} /></h1>

      <section className="card">
        <h2>Problem</h2>
        <Markdown text={question.problem} />
      </section>

      <section>
        <h2>{question.approaches.length > 1 ? 'Approaches: worst → best' : 'Approach'}</h2>

        {question.approaches.length > 1 && hasComplexity && (
          <div className="table-wrap">
            <table className="summary-table">
              <thead><tr><th>#</th><th>Approach</th><th>Time</th><th>Space</th></tr></thead>
              <tbody>
                {question.approaches.map((approach, i) => (
                  <tr key={approach.name}><td>{i + 1}</td><td>{approach.name}</td><td>{approach.time}</td><td>{approach.space}</td></tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {question.sharedCode && (
          <div className="card approach">
            <div className="approach-head"><h3>Shared code</h3></div>
            <p className="muted">Helpers used by the approaches below.</p>
            <CodeBlock code={question.sharedCode} />
          </div>
        )}

        {question.approaches.map((approach, i) => (
          <div className="card approach" key={approach.name}>
            <div className="approach-head">
              <h3>{question.approaches.length > 1 && `${i + 1}. `}{approach.name}</h3>
              {i === question.approaches.length - 1 && question.approaches.length > 1 && <span className="best">Best</span>}
              {approach.time && <span className="complexity">Time {approach.time}</span>}
              {approach.space && <span className="complexity">Space {approach.space}</span>}
            </div>
            <Markdown text={approach.idea} />
            <CodeBlock code={approach.code} />
          </div>
        ))}
      </section>

      <section className="card">
        <h2>Result</h2>
        {question.result.examples.map((example, i) => (
          <div className="example" key={i}>
            <div><span className="label">Input</span><code>{example.input}</code></div>
            <div><span className="label">Output</span><code>{example.output}</code></div>
          </div>
        ))}
        {question.result.text && <pre className="result-text">{question.result.text}</pre>}
        {question.result.errors.map((message, i) => <pre key={i} className="error">{message}</pre>)}
      </section>

      <nav className="pager">
        {previous ? <a className="button ghost" href={`#/q/${previous.id}`}>← {previous.title}</a> : <span />}
        {next && <a className="button ghost" href={`#/q/${next.id}`}>{next.title} →</a>}
      </nav>
    </article>
  )
}
