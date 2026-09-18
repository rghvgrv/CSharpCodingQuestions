import { useEffect, useState } from 'react'

// Hash routes: #/ (home), #/topic/Dsa/Arrays, #/q/TwoSum
function parse(hash) {
  if (hash.startsWith('#/q/')) return { page: 'question', id: decodeURIComponent(hash.slice(4)) }
  if (hash.startsWith('#/topic/')) return { page: 'topic', id: decodeURIComponent(hash.slice(8)) }
  return { page: 'home', id: null }
}

export default function useRoute() {
  const [hash, setHash] = useState(location.hash)
  useEffect(() => {
    const onChange = () => setHash(location.hash)
    addEventListener('hashchange', onChange)
    return () => removeEventListener('hashchange', onChange)
  }, [])
  return parse(hash)
}
