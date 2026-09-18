import { useMemo, useState } from 'react'
import hljs from 'highlight.js/lib/core'
import csharp from 'highlight.js/lib/languages/csharp'
import 'highlight.js/styles/github-dark.css'

hljs.registerLanguage('csharp', csharp)

async function copyToClipboard(text) {
  if (navigator.clipboard && window.isSecureContext) {
    await navigator.clipboard.writeText(text)
    return
  }
  // A phone opening http://<pc-ip>:5080 is not a "secure context", so the Clipboard API is missing there.
  const textArea = document.createElement('textarea')
  textArea.value = text
  textArea.readOnly = true
  textArea.style.position = 'fixed'
  textArea.style.opacity = '0'
  document.body.appendChild(textArea)
  textArea.select()
  textArea.setSelectionRange(0, text.length)
  document.execCommand('copy')
  textArea.remove()
}

export default function CodeBlock({ code }) {
  const [copied, setCopied] = useState(false)
  const html = useMemo(() => hljs.highlight(code, { language: 'csharp' }).value, [code])

  async function handleCopy() {
    await copyToClipboard(code)
    setCopied(true)
    setTimeout(() => setCopied(false), 1500)
  }

  return (
    <div className="code-block">
      <button className="copy-button" onClick={handleCopy}>{copied ? 'Copied ✓' : 'Copy'}</button>
      <pre className="hljs"><code dangerouslySetInnerHTML={{ __html: html }} /></pre>
    </div>
  )
}
