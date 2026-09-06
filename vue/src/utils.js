/* ============================================================================
   RizUI — 工具函数
   ========================================================================== */

/**
 * 打开链接（新标签页）
 * 用「合成 <a> 点击」模拟真实用户手势，避免 window.open 在
 * Teleport / 弹窗等场景下被浏览器弹窗拦截。
 * @param {string} url    要打开的地址
 * @param {string} target 打开方式，默认 _blank
 */
export function openURL(url, target = '_blank') {
  const a = document.createElement('a')
  a.href = url
  a.target = target
  a.rel = 'noopener noreferrer'
  document.body.appendChild(a)
  a.click()
  a.remove()
}
