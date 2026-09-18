import { useEffect, useState } from 'react'
import { getJson } from './api.js'
import useRoute from './useRoute.js'
import Sidebar from './components/Sidebar.jsx'
import HomePage from './components/HomePage.jsx'
import TopicPage from './components/TopicPage.jsx'
import QuestionPage from './components/QuestionPage.jsx'

export default function App() {
  const [catalog, setCatalog] = useState(null)
  const [error, setError] = useState(null)
  const route = useRoute()

  useEffect(() => {
    getJson('/api/catalog').then(setCatalog).catch(e => setError(e.message))
  }, [])

  useEffect(() => {
    window.scrollTo(0, 0)
    document.querySelector('main')?.scrollTo(0, 0)
  }, [route.page, route.id])

  const allQuestions = catalog ? catalog.sections.flatMap(s => s.topics.flatMap(t => t.questions)) : []

  return (
    <div className={route.page === 'home' ? 'app' : 'app is-reading'}>
      <aside>
        <Sidebar catalog={catalog} route={route} />
      </aside>
      <main>
        {error && <p className="error">Could not load the questions: {error}</p>}
        {catalog && route.page === 'home' && <HomePage catalog={catalog} />}
        {catalog && route.page === 'topic' && <TopicPage key={route.id} topicId={route.id} catalog={catalog} />}
        {catalog && route.page === 'question' && <QuestionPage key={route.id} questionId={route.id} allQuestions={allQuestions} />}
      </main>
    </div>
  )
}
