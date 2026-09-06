<script setup>
import { inject } from 'vue'
import { UiCard } from 'riz-ui'
const themeColor = inject('themeColor')
</script>

<template>
  <main class="main pricing-page">
    <section class="sec-header">
      <h2>许可协议</h2>
      <p class="sec-desc">RizUI 采用 AGPLv3 协议开源，同时提供商业许可选项。</p>
    </section>
    <UiCard title="开源许可 — AGPLv3" desc="适用于开源项目或个人学习">
      <div class="pricing-list">
        <div class="pricing-row"><span>OK</span> 自由使用、修改、分发</div>
        <div class="pricing-row"><span>OK</span> 查看全部源代码</div>
        <div class="pricing-row"><span>OK</span> 社区 Issue 支持</div>
        <div class="pricing-row"><span>WARN</span> 衍生作品必须同样以 AGPLv3 开源</div>
        <div class="pricing-row"><span>WARN</span> 网络服务使用也需公开源码</div>
      </div>
    </UiCard>
    <UiCard title="商业许可" desc="适用于不希望开源的闭源商业项目">
      <div class="pricing-list">
        <div class="pricing-row"><span>OK</span> 闭源使用，无需公开代码</div>
        <div class="pricing-row"><span>OK</span> 永久授权，不限项目数</div>
        <div class="pricing-row"><span>OK</span> 优先技术支持</div>
        <div class="pricing-row"><span>OK</span> 法律保障，无 AGPL 风险</div>
      </div>
      <div style="margin-top:16px;padding:14px;background:#fafbfc;border-radius:10px;text-align:center">
        <p style="font-size:13px;color:#667;margin-bottom:8px">如需商业许可，请联系：</p>
        <a href="mailto:YunHydrogenP-w@qq.com" :style="{color:themeColor,fontSize:'14px',fontWeight:700,textDecoration:'none'}">YunHydrogen@outlook.com</a>
      </div>
    </UiCard>
    <UiCard title="AGPLv3 协议全文" desc="">
      <p style="font-size:12px;color:#99a;line-height:1.8">
        GNU Affero General Public License v3.0<br/>
        本软件按"原样"提供，不提供任何形式的明示或暗示担保。<br/>
        完整协议见：<a href="https://www.gnu.org/licenses/agpl-3.0.html" target="_blank" :style="{color:themeColor}">https://www.gnu.org/licenses/agpl-3.0.html</a>
      </p>
    </UiCard>
  </main>
</template>
