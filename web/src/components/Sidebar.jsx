import { useEffect, useRef, useState } from 'react'

const LEVELS = ['All', 'Easy', 'Medium', 'Hard']

export default function Sidebar({ catalog, route }) {
  const [search, setSearch] = useState('')
  const [level, setLevel] = useState('All')
  const activeLink = useRef(null)

  useEffect(() => {
    activeLink.current?.scrollIntoView({ block: 'nearest' })
  }, [route.id, catalog])

  const query = search.trim().toLowerCase()
  const isFiltering = query !== '' || level !== 'All'
  const matches = (question, topic) =>
    (level === 'All' || question.level === level) &&
    (query === '' ||
      String(question.number) === query.replace('#', '') ||
      `${question.title} ${topic.title}`.toLowerCase().includes(query))

  const questionCount = catalog?.sections.reduce((sum, s) => sum + s.topics.reduce((n, t) => n + t.questions.length, 0), 0)

  return (
    <>
      <header className="brand">
        <a href="#/"><span className="logo">C#</span> Coding Questions</a>
        <small>{questionCount ?? '…'} questions · DSA · Tree · Parallelism</small>
      </header>

      <div className="filters">
        <input type="search" placeholder="Search questions…" value={search} onChange={e => setSearch(e.target.value)} />
        <div className="chips">
          {LEVELS.map(name => (
            <button key={name} className={name === level ? 'chip selected' : 'chip'} onClick={() => setLevel(name)}>{name}</button>
          ))}
        </div>
      </div>

      <nav>
        {catalog?.sections.map(section => (
          <section key={section.title}>
            <h2>{section.title}</h2>
            {section.topics.map(topic => {
              const questions = topic.questions.filter(q => matches(q, topic))
              if (isFiltering && questions.length === 0) return null
              const isCurrentTopic = route.id === topic.id || topic.questions.some(q => q.id === route.id)
              return (
                <details key={topic.id} open={isFiltering || isCurrentTopic}>
                  <summary>{topic.title}<span className="count">{topic.questions.length}</span></summary>
                  <ol>
                    {!isFiltering && (
                      <li>
                        <a href={`#/topic/${topic.id}`} className={route.id === topic.id ? 'intro active' : 'intro'} ref={route.id === topic.id ? activeLink : null}>
                          <span className="number">📖</span> Introduction
                        </a>
                      </li>
                    )}
                    {questions.map(question => (
                      <li key={question.id}>
                        <a href={`#/q/${question.id}`} className={route.id === question.id ? 'active' : ''} ref={route.id === question.id ? activeLink : null}>
                          <span className="number">{question.number}</span>
                          <span className="title">{question.title}</span>
                          <span className={`dot ${question.level.toLowerCase()}`} title={question.level} />
                        </a>
                      </li>
                    ))}
                  </ol>
                </details>
              )
            })}
          </section>
        ))}
      </nav>
    </>
  )
}
