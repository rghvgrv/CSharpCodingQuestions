export async function getJson(url) {
  const response = await fetch(url)
  if (!response.ok) throw new Error(`${response.status} ${response.statusText}`)
  return response.json()
}
