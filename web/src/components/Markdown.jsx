import CodeBlock from './CodeBlock.jsx'

// Renders the small part of Markdown used by topic and question text:
// ## headings, paragraphs, - and 1. lists, | tables |, ``` code blocks ```, `inline code` and **bold**.
const LIST_ITEM = /^\s*([-*]|\d+\.) /
const BLOCK_START = /^(```|#{1,4} |\s*([-*]|\d+\.) |\|)/

export default function Markdown({ text }) {
  const lines = text.replace(/\r/g, '').split('\n')
  const blocks = []
  let i = 0

  while (i < lines.length) {
    const line = lines[i]
    const key = blocks.length

    if (line.trim() === '') {
      i++
    } else if (line.startsWith('```')) {
      const isDiagram = line.slice(3).trim() === 'text'
      const code = []
      for (i++; i < lines.length && !lines[i].startsWith('```'); i++) code.push(lines[i])
      i++
      blocks.push(isDiagram
        ? <pre key={key} className="diagram">{code.join('\n')}</pre>
        : <CodeBlock key={key} code={code.join('\n')} />)
    } else if (/^#{1,4} /.test(line)) {
      const level = line.indexOf(' ')
      const Heading = `h${Math.max(level, 2)}`
      blocks.push(<Heading key={key}>{renderInline(line.slice(level + 1))}</Heading>)
      i++
    } else if (LIST_ITEM.test(line)) {
      const ordered = /^\s*\d+\./.test(line)
      const items = []
      for (; i < lines.length && LIST_ITEM.test(lines[i]); i++) items.push(lines[i].replace(LIST_ITEM, ''))
      const List = ordered ? 'ol' : 'ul'
      blocks.push(<List key={key}>{items.map((item, n) => <li key={n}>{renderInline(item)}</li>)}</List>)
    } else if (line.startsWith('|')) {
      const rows = []
      for (; i < lines.length && lines[i].startsWith('|'); i++) {
        if (/^\|[\s:|-]+\|?$/.test(lines[i].trim())) continue // the |---|---| separator row
        // Split on | but not on an escaped \| (used for the C# OR operator inside a cell).
        const cells = lines[i].trim().replace(/^\||\|$/g, '').split(/(?<!\\)\|/)
        rows.push(cells.map(cell => cell.trim().replaceAll('\\|', '|')))
      }
      blocks.push(
        <div key={key} className="table-wrap">
          <table>
            <thead><tr>{rows[0].map((cell, n) => <th key={n}>{renderInline(cell)}</th>)}</tr></thead>
            <tbody>{rows.slice(1).map((row, r) => <tr key={r}>{row.map((cell, n) => <td key={n}>{renderInline(cell)}</td>)}</tr>)}</tbody>
          </table>
        </div>
      )
    } else {
      const paragraph = []
      for (; i < lines.length && lines[i].trim() !== '' && !BLOCK_START.test(lines[i]); i++) paragraph.push(lines[i].trim())
      blocks.push(<p key={key}>{renderInline(paragraph.join(' '))}</p>)
    }
  }

  return <div className="markdown">{blocks}</div>
}

function renderInline(text) {
  return text.split(/(`[^`]+`|\*\*[^*]+\*\*)/).map((part, i) => {
    if (part.startsWith('`') && part.endsWith('`') && part.length > 1) return <code key={i}>{part.slice(1, -1)}</code>
    if (part.startsWith('**') && part.endsWith('**') && part.length > 3) return <strong key={i}>{part.slice(2, -2)}</strong>
    return part
  })
}
