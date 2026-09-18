import { useEffect, useState } from 'react'
import { getJson } from '../api.js'
import Markdown from './Markdown.jsx'
import LevelBadge from './LevelBadge.jsx'

export default function TopicPage({ topicId, catalog }) {
  const [topic, setTopic] = useState(null)
  const [error, setError] = useState(null)

  useEffect(() => {
    getJson(`data/topics/${topicId}.json`).then(setTopic).catch(e => setError(e.message))
  }, [topicId])

  const questions = catalog.sections.flatMap(s => s.topics).find(t => t.id === topicId)?.questions ?? []

  if (error) return <div className="page error">Topic not found.</div>
  if (!topic) return <div className="page muted">Loading…</div>

  return (
    <article className="page">
      <a href="#/" className="back">← All topics</a>
      <div className="crumbs">{topic.sectionTitle}</div>
      <h1>{topic.title}</h1>

      <section className="card">
        <Markdown text={topic.body} />
      </section>

      <section>
        <h2>Questions</h2>
        <ol className="question-list">
          {questions.map(question => (
            <li key={question.id}>
              <a href={`#/q/${question.id}`}>
                <span className="number">#{question.number}</span>
                <span className="title">{question.title}</span>
                <LevelBadge level={question.level} />
              </a>
            </li>
          ))}
        </ol>
        {questions[0] && <a className="button" href={`#/q/${questions[0].id}`}>Start the first question →</a>}
      </section>
    </article>
  )
}
