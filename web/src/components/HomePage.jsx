export default function HomePage({ catalog }) {
  const firstTopic = catalog.sections[0]?.topics[0]

  return (
    <div className="page">
      <h1>C# Coding Questions</h1>
      <p className="lead">
        Learn data structures, algorithms, trees and parallel programming step by step, in C#.
        Every topic starts with a simple explanation. Every question then shows several solutions,
        from the slowest to the fastest, so you can see <em>why</em> the best one is better.
      </p>

      <div className="how-it-works">
        <div><b>1. Read the topic</b><span>What it is and when to use it</span></div>
        <div><b>2. Understand the problem</b><span>In plain words</span></div>
        <div><b>3. Compare the approaches</b><span>From worst to best, with Copy</span></div>
        <div><b>4. See the result</b><span>Real output of the code</span></div>
      </div>

      {firstTopic && <a className="button" href={`#/topic/${firstTopic.id}`}>Start learning →</a>}

      {catalog.sections.map(section => (
        <section key={section.title}>
          <h2>{section.title}</h2>
          <div className="topic-grid">
            {section.topics.map(topic => (
              <a key={topic.id} className="topic-card" href={`#/topic/${topic.id}`}>
                <b>{topic.title}</b>
                <span>{topic.summary}</span>
                <small>{topic.questions.length} questions</small>
              </a>
            ))}
          </div>
        </section>
      ))}
    </div>
  )
}
