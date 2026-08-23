<script setup>
import { ref } from 'vue'
import { Ctnr_Toast, Ctnr_Dialog, Cntr_Card, Cntr_Cfgrow, Cntr_Infobox, Ctrl_Button, openURL } from 'riz-ui'
import Inpt_Text from '../../../../src/components/Input/Inpt_Text.vue'

const showInfo = ref(false)
const showWarn = ref(false)
const showError = ref(false)
const showNoBar = ref(false)
const showNoClose = ref(false)
const showPersist = ref(false)
const showColor = ref(false)
const showSlot = ref(false)
const showQueue = ref(false)
const persistRef = ref(null)
const showClick = ref(false)
const clickDialog = ref(false)
const showLong = ref(false)
const sendMsg = ref(false)

const props = [
  { name:'type',         type:'String',  default:'"info"',  desc:'info / warn / error' },
  { name:'title',        type:'String',  default:'""',      desc:'标题，可空' },
  { name:'hint',         type:'String',  default:'""',      desc:'说明文字，可空' },
  { name:'titleInstyle', type:'Object',  default:'{}',      desc:'标题样式覆盖' },
  { name:'hintInstyle',  type:'Object',  default:'{}',      desc:'说明文字样式覆盖' },
  { name:'icon',         type:'String',  default:'""',      desc:'自定义图标，空时按 type' },
  { name:'iconInstyle',  type:'Object',  default:'{}',      desc:'图标样式覆盖' },
  { name:'countdown',    type:'Number',  default:'3',       desc:'自动关闭秒数，0 不自动关' },
  { name:'closable',     type:'Boolean', default:'true',    desc:'显示关闭按钮' },
  { name:'showBar',      type:'Boolean', default:'true',    desc:'显示底部倒计时进度条' },
  { name:'color',        type:'String',  default:'""',      desc:'仅 info 生效，空则跟随主题色' },
  { name:'teleport',     type:'String',  default:'"body"',  desc:'挂载目标（选择器），默认 body' },
  { name:'width',        type:'String',  default:'""',      desc:'宽度（如 420px），空时默认 320px' }
]
const events = ['click — 点击通知本体触发（不自动关闭，可绑定其他行为）']
const usage = `<!-- title + hint：自闭合用法 -->
<Ctnr_Toast v-if="show" type="info" title="保存成功" hint="改动已写进文件。"
            @close="show = false" />

<!-- 默认插槽：可以塞更丰富的内容 -->
<Ctnr_Toast v-if="show" type="warn" title="检查一下"
            @close="show = false">
  请 <b>仔细</b> 检查配置再继续。
</Ctnr_Toast>

<!-- 自定义颜色：进度条和图标跟着变 -->
<Ctnr_Toast v-if="show" type="info" title="自定义色" hint="进度条也用这个色"
            color="#8b5cf6" @close="show = false" />

<!-- 自定义挂载目标：和其他控件一样用 teleport（默认 body） -->
<Ctnr_Toast v-if="show" type="info" title="挂到别处" hint="teleport 指定选择器"
            teleport="#app" @close="show = false" />

<!-- 自定义宽度：width prop -->
<Ctnr_Toast v-if="show" type="info" title="宽一点" hint="width=420px"
            width="420px" @close="show = false" />

<!-- 点击是独立事件：可绑定关闭，也能绑定其他行为（配合 ref 暴露的 close） -->
<Ctnr_Toast v-if="show" ref="tRef" type="info" title="点一下" hint="click 事件由你决定做什么"
            :countdown="0" @click="tRef.close()" @close="show = false" />`
</script>

<template>
  <main class="main">
    <section class="sec-header">
      <div class="sec-title-row"><h2>Toast 通知</h2><p class="sec-tag">Ctnr_Toast</p></div>
      <p class="sec-desc">右上角的一个Toast的通知</p>
    </section>

    <section class="sec-demo"><h3><i class="fa-solid fa-gamepad"></i> 示例</h3>

      <Cntr_Card title="预设样式" desc="type 可为 info / warn / error">
        <Cntr_Cfgrow label="普通Toast" hint="点击按钮弹出一个普通的Toast">
          <Ctrl_Button text="弹Toast" primary @click="showInfo = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="警告Toast" hint="点击按钮弹出一个警告样式的Toast">
          <Ctrl_Button text="弹Toast" @click="showWarn = true" r-icon="fa-solid fa-rocket"  color="#efb134" style="background-color:#e6d15b;"/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="错误Toast" hint="点击按钮弹出一个错误样式的Toast">
          <Ctrl_Button text="弹Toast" danger @click="showError = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="自定义Toast" hint="此Toast使用了自定义颜色#e56161，当类型为 info 时，color prop 可自定义颜色">
          <Ctrl_Button text="弹Toast" @click="showColor = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>        
      </Cntr_Card>

      <Cntr_Card title="可配置功能" desc="关进度条 / 关关闭按钮 / 不自动关">
        <Cntr_Cfgrow label="没有进度条的Toast" hint="showBar=false">
          <Ctrl_Button text="弹Toast" @click="showNoBar = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="没有关闭按钮的Toast" hint="closable=false，等待倒计时结束自动关闭">
          <Ctrl_Button text="弹Toast" @click="showNoClose = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="不自动关闭的Toast" hint="countdown=0，需要手动关闭（关闭按钮或点击本体，关闭事件绑定在 click 事件上）">
          <Ctrl_Button text="弹Toast" @click="showPersist = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="30s倒计时的Toast" hint="countdown=30，倒计时很长">
          <Ctrl_Button text="弹Toast" @click="showLong = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>
        <Cntr_Cfgrow label="带插槽的Toast" hint="title/hint 下方排一段内容">
          <Ctrl_Button text="弹Toast" @click="showSlot = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>
      </Cntr_Card>

      <Cntr_Card title="排队栈" desc="自动在右上角上下排队">
        <Cntr_Cfgrow label="一键三连" hint="共享一个排队栈">
          <Ctrl_Button text="弹Toast" @click="showQueue = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>
      </Cntr_Card>

      <Cntr_Card title="自定义click事件" desc="click 事件可以绑定各种各样的行为哦">
        <Cntr_Cfgrow label="点击领取24000青辉石" hint="为了庆祝三周年，特意为老师准备了24000青辉石！">
          <Ctrl_Button text="弹Toast" @click="showClick = true" r-icon="fa-solid fa-rocket"/>
        </Cntr_Cfgrow>
      </Cntr_Card>

      <!-- Toast 实例 -->
      <Ctnr_Toast v-if="showInfo" type="info" title="欢迎回来！" hint="老师，今天你想从哪件事开始做起呢...?"
                  @close="showInfo = false" />
      <Ctnr_Toast v-if="showWarn" type="warn" title="突击学员撤退！" hint="当有突击学员撤退时，技能蓄能值回复速率将减少。"
                  @close="showWarn = false" />
      <Ctnr_Toast v-if="showError" type="error" title="战败" hint="在下次尝试之前，先提升学员的属性值吧。"
                  @close="showError = false" />
      <Ctnr_Toast v-if="showColor" type="info" title="让世界变得更有趣吧！" hint="这是使用了自定义颜色的toast哦！" color="#e56161"
                  @close="showColor = false" />

      <Ctnr_Toast v-if="showNoBar" type="info" title="任务开始！" hint="请击败敌方首领！"
                  :show-bar="false" @close="showNoBar = false" />
      <Ctnr_Toast v-if="showNoClose" type="info" title="来自MomoTalk的信息" hint="老师，你能狡辩一下这笔沙勒的支出吗?!" color="#e278b3"
                  :closable="false" @close="showNoClose = false" />
      <Ctnr_Toast v-if="showPersist" ref="persistRef" type="error" title="总力战已开启！" hint="快去准备部队迎战吧！"
                  :countdown="0" @click="persistRef?.close()"
                  @close="showPersist = false" />
      <Ctnr_Toast v-if="showLong" type="info" title="剩余时间" hint="请在战斗结束前击败敌方首领！"
                  :countdown="30" @close="showLong = false" />
      <Ctnr_Toast v-if="showSlot" type="info" title="Momotalk" hint="爱丽丝：邦邦卡邦！老师快来游戏开发部一趟！" icon="fa-solid fa-message" width="390px"
                  @close="showSlot = false" :countdown="0">
        <Inpt_Text placeholder="在此快速回复..." style="width: 80%;" @enter="sendMsg = true; showSlot = false"/>
        <Ctrl_Button primary r-icon="fa-solid fa-paper-plane" style="margin-left: 10px;" @click="sendMsg = true; showSlot = false"/>
      </Ctnr_Toast>
      <Ctnr_Toast v-if="sendMsg" type="info" title="发送成功" hint="消息已发送!"
                  @close="sendMsg = false" />


      <Ctnr_Toast v-if="showQueue" type="info" title="关注YunHydrogenP喵谢谢喵" hint="点击下方链接跳转到哔哩哔哩"
                  @close="showQueue = false" icon="fa-brands fa-bilibili" countdown="10">
        <Ctrl_Button text="点我跳转" primary r-icon="fa-solid fa-arrow-up-right-from-square" long @click="openURL('https://space.bilibili.com/560229787')" />
      </Ctnr_Toast>
      <Ctnr_Toast v-if="showQueue" type="info" title="关注HZYZ算法AI社喵谢谢喵" hint="点击下方链接跳转到哔哩哔哩"
                  @close="showQueue = false" icon="fa-brands fa-bilibili" countdown="10">
      <Ctrl_Button text="点我跳转" primary r-icon="fa-solid fa-arrow-up-right-from-square" long @click="openURL('https://space.bilibili.com/3546964417316900')" />
      </Ctnr_Toast>
      <Ctnr_Toast v-if="showQueue" type="info" title="关注HZYZ智能信息社喵谢谢喵" hint="点击下方链接跳转到哔哩哔哩"
                  @close="showQueue = false" icon="fa-brands fa-bilibili" countdown="10">
        <Ctrl_Button text="点我跳转" primary r-icon="fa-solid fa-arrow-up-right-from-square" long @click="openURL('https://space.bilibili.com/3546799325317275')" />
      </Ctnr_Toast>


      <Ctnr_Toast v-if="showClick" type="info" title="老师，这是我给您准备的特别礼物哦！" hint="但是要拼手速哦！" icon="fa-solid fa-gift" width="auto"
                  :countdown="0.5" @click="clickDialog = true" @close="showClick = false"/>
      <Ctnr_Dialog v-model:show="clickDialog" title="谨 防 上 当 受 骗" icon="fa-solid fa-bell" width="400px">
        <iframe src="//player.bilibili.com/player.html?isOutside=true&aid=80433022&bvid=BV1GJ411x7h7&cid=137649199&p=1&muted=false&danmaku=false" scrolling="no" border="0" frameborder="no" framespacing="0" allowfullscreen="false" style="width: 100%; aspect-ratio: 16 / 9; display: block; border-radius: 16px;"></iframe>
        <template #footer>
          <Ctrl_Button text="知道了" primary @click="clickDialog = false" />
        </template>
      </Ctnr_Dialog>

    </section>

    <section class="sec-props"><h3><i class="fa-solid fa-list-check"></i> Props</h3>
      <table class="props-table"><thead><tr><th>属性</th><th>类型</th><th>必填</th><th>默认值</th><th>说明</th></tr></thead>
        <tbody><tr v-for="p in props" :key="p.name"><td><code>{{ p.name }}</code></td><td><code>{{ p.type }}</code></td><td>{{ p.required||'—' }}</td><td><code>{{ p.default||'—' }}</code></td><td>{{ p.desc }}</td></tr></tbody>
      </table>
    </section>

    <!-- 主题色提示 -->
    <Cntr_Infobox type="info">
      <code>info</code> 默认跟随全局变量 <code>--rizui_themecolor</code>（默认 <code>#66CCFF</code>），<code>color</code> 可自定义（仅 info 生效）；warn / error 用预设色。试试右上角取色器实时换色，或在你的项目中于 <code>:root</code> 覆盖该变量。
    </Cntr_Infobox>

    <section class="sec-events"><h3><i class="fa-solid fa-satellite-dish"></i> Events</h3>
      <ul><li v-for="e in events" :key="e"><code>{{ e }}</code></li></ul>
    </section>

    <section class="sec-usage"><h3><i class="fa-solid fa-code"></i> 用法</h3>
      <pre class="code-block"><code>{{ usage }}</code></pre>
    </section>
  </main>
</template>
