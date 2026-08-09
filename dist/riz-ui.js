import { openBlock as r, createElementBlock as m, createElementVNode as l, normalizeStyle as g, normalizeClass as N, createCommentVNode as R, toDisplayString as C, renderSlot as q, computed as b, ref as x, onMounted as G, onBeforeUnmount as J, withModifiers as U, useCssVars as oe, createVNode as Q, Transition as ae, withCtx as se, Fragment as W, renderList as Z, createBlock as ce, Teleport as ie } from "vue";
const F = (t, p) => {
  const e = t.__vccOpts || t;
  for (const [$, v] of p)
    e[$] = v;
  return e;
}, re = { class: "ct-card" }, de = { class: "ct-card-head" }, me = { class: "ct-card-head-text" }, fe = {
  key: 0,
  class: "ct-card-body"
}, ve = {
  __name: "Cntr_Card",
  props: {
    title: { type: String, default: "" },
    titleInstyle: { type: Object, default: () => ({}) },
    desc: { type: String, default: "" },
    descInstyle: { type: Object, default: () => ({}) },
    icon: { type: String, default: "" },
    iconInstyle: { type: Object, default: () => ({}) }
  },
  setup(t) {
    return (p, e) => (r(), m("div", re, [
      l("div", de, [
        t.icon ? (r(), m("div", {
          key: 0,
          class: "ct-card-icon",
          style: g(t.iconInstyle)
        }, [
          l("i", {
            class: N(t.icon)
          }, null, 2)
        ], 4)) : R("", !0),
        l("div", me, [
          t.title ? (r(), m("div", {
            key: 0,
            class: "ct-card-title",
            style: g(t.titleInstyle)
          }, C(t.title), 5)) : R("", !0),
          t.desc ? (r(), m("div", {
            key: 1,
            class: "ct-card-desc",
            style: g(t.descInstyle)
          }, C(t.desc), 5)) : R("", !0)
        ])
      ]),
      p.$slots.default ? (r(), m("div", fe, [
        q(p.$slots, "default", {}, void 0, !0)
      ])) : R("", !0)
    ]));
  }
}, St = /* @__PURE__ */ F(ve, [["__scopeId", "data-v-ca4cde43"]]), pe = { class: "cfgr-head" }, he = { class: "cfgr-head-text" }, ye = {
  key: 0,
  class: "cfgr-slot-wrap"
}, ge = {
  __name: "Cntr_Cfgrow",
  props: {
    label: { type: String, default: "" },
    labelInstyle: { type: Object, default: () => ({}) },
    hint: { type: String, default: "" },
    hintInstyle: { type: Object, default: () => ({}) },
    icon: { type: String, default: "" },
    iconInstyle: { type: Object, default: () => ({}) },
    stack: { type: Boolean, default: !1 },
    disabled: { type: Boolean, default: !1 }
  },
  setup(t) {
    return (p, e) => (r(), m("div", {
      class: N(["cfgr-row", { "cfgr-stack": t.stack, "cfgr-disabled": t.disabled }])
    }, [
      l("div", pe, [
        t.icon ? (r(), m("div", {
          key: 0,
          class: "cfgr-icon",
          style: g(t.iconInstyle)
        }, [
          l("i", {
            class: N(t.icon)
          }, null, 2)
        ], 4)) : R("", !0),
        l("div", he, [
          t.label ? (r(), m("div", {
            key: 0,
            class: "cfgr-label",
            style: g(t.labelInstyle)
          }, C(t.label), 5)) : R("", !0),
          t.hint ? (r(), m("div", {
            key: 1,
            class: "cfgr-hint",
            style: g(t.hintInstyle)
          }, C(t.hint), 5)) : R("", !0)
        ]),
        t.stack ? R("", !0) : (r(), m("div", {
          key: 1,
          class: N(["cfgr-slot-inner", { "cfgr-slot-disabled": t.disabled }])
        }, [
          q(p.$slots, "default", {}, void 0, !0)
        ], 2))
      ]),
      t.stack ? (r(), m("div", ye, [
        l("div", {
          class: N(["cfgr-slot-inner", { "cfgr-slot-disabled": t.disabled }])
        }, [
          q(p.$slots, "default", {}, void 0, !0)
        ], 2)
      ])) : R("", !0)
    ], 2));
  }
}, wt = /* @__PURE__ */ F(ge, [["__scopeId", "data-v-f5b590b9"]]), be = ["checked", "disabled"], ke = {
  __name: "Ctrl_Switch",
  props: {
    modelValue: { type: Boolean, default: !1 },
    color: { type: String, default: "#39c5bb" },
    disabled: { type: Boolean, default: !1 }
  },
  emits: ["update:modelValue"],
  setup(t) {
    const p = t, e = b(() => ({
      background: p.modelValue ? p.color : void 0
    }));
    return ($, v) => (r(), m("label", {
      class: N(["csw-root", { "csw-disabled": t.disabled }])
    }, [
      l("input", {
        type: "checkbox",
        checked: t.modelValue,
        disabled: t.disabled,
        onChange: v[0] || (v[0] = (i) => $.$emit("update:modelValue", i.target.checked))
      }, null, 40, be),
      l("span", {
        class: "csw-track",
        style: g(e.value)
      }, null, 4)
    ], 2));
  }
}, Et = /* @__PURE__ */ F(ke, [["__scopeId", "data-v-d03b0c2c"]]), Ve = { class: "csl-value" }, $e = 400, Me = 60, xe = {
  __name: "Ctrl_Slider",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "#39c5bb" },
    display: { type: String, default: "" },
    displayValue: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: p }) {
    const e = t, $ = p, v = x(null), i = b(() => {
      const u = e.max - e.min;
      return u <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / u * 100));
    }), M = b(() => ({
      background: `linear-gradient(to right, ${e.color}33 ${i.value}%, transparent ${i.value}%)`,
      borderColor: e.color
    }));
    function T(u) {
      return e.step < 1 ? parseFloat(u) : parseInt(u);
    }
    function E(u) {
      const o = e.modelValue + u * e.step, a = Math.max(e.min, Math.min(e.max, o)), s = T(a.toFixed(10));
      s !== e.modelValue && $("update:modelValue", s);
    }
    const _ = x(!1);
    function S(u) {
      if (!v.value) return e.modelValue;
      const o = v.value.getBoundingClientRect(), a = Math.max(0, Math.min(1, (u - o.left) / o.width)), s = e.min + a * (e.max - e.min), f = Math.round(s / e.step) * e.step;
      return T(Math.max(e.min, Math.min(e.max, f)).toFixed(10));
    }
    function h(u) {
      var f, L;
      _.value = !0;
      const o = u.target, a = o.closest(".csl-minus"), s = o.closest(".csl-plus");
      if (a)
        E(-1), P(-1);
      else if (s)
        E(1), P(1);
      else {
        const O = S(u.clientX || ((L = (f = u.touches) == null ? void 0 : f[0]) == null ? void 0 : L.clientX) || 0);
        O !== e.modelValue && $("update:modelValue", O);
      }
    }
    function k(u) {
      var a, s;
      if (!_.value) return;
      X();
      const o = S(u.clientX || ((s = (a = u.touches) == null ? void 0 : a[0]) == null ? void 0 : s.clientX) || 0);
      o !== e.modelValue && $("update:modelValue", o);
    }
    function V() {
      _.value = !1, X();
    }
    const w = x(null);
    function P(u) {
      X(), w.value = setTimeout(() => {
        w.value = setInterval(() => E(u), Me);
      }, $e);
    }
    function X() {
      w.value && (clearTimeout(w.value), clearInterval(w.value), w.value = null);
    }
    G(() => {
      document.addEventListener("mousemove", k), document.addEventListener("mouseup", V), document.addEventListener("touchmove", k, { passive: !1 }), document.addEventListener("touchend", V);
    }), J(() => {
      document.removeEventListener("mousemove", k), document.removeEventListener("mouseup", V), document.removeEventListener("touchmove", k), document.removeEventListener("touchend", V), X();
    });
    const D = b(() => {
      if (e.displayValue) return e.displayValue;
      const u = e.step < 1 ? e.modelValue.toFixed(1) : e.modelValue;
      return e.display ? `${u}${e.display}` : String(u);
    });
    return (u, o) => (r(), m("div", {
      ref_key: "rootRef",
      ref: v,
      class: N(["csl-root", { "csl-dragging": _.value }]),
      style: g(M.value),
      onMousedown: U(h, ["left"]),
      onTouchstart: U(h, ["prevent"])
    }, [
      o[0] || (o[0] = l("button", { class: "csl-btn csl-minus" }, "−", -1)),
      l("span", Ve, C(D.value), 1),
      o[1] || (o[1] = l("button", { class: "csl-btn csl-plus" }, "+", -1))
    ], 38));
  }
}, Ct = /* @__PURE__ */ F(xe, [["__scopeId", "data-v-5b0d4fc0"]]), _e = {
  key: 0,
  class: "cdd-drop"
}, Se = ["onClick"], we = {
  __name: "Ctrl_Dropdown",
  props: {
    modelValue: { type: [String, Number], default: "" },
    options: { type: Array, default: () => [] },
    color: { type: String, default: "#39c5bb" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: p }) {
    oe((h) => ({
      d9f9d292: e.color
    }));
    const e = t, $ = p, v = x(null), i = x(!1), M = b(() => {
      const h = e.options.find((k) => k.value === e.modelValue);
      return h ? h.label : String(e.modelValue || "");
    }), T = b(() => ({
      color: e.color,
      background: e.color + "14"
    }));
    function E() {
      i.value = !i.value;
    }
    function _(h) {
      $("update:modelValue", h), i.value = !1;
    }
    function S(h) {
      v.value && !v.value.contains(h.target) && (i.value = !1);
    }
    return G(() => {
      document.addEventListener("mousedown", S, !0), document.addEventListener("touchstart", S, !0);
    }), J(() => {
      document.removeEventListener("mousedown", S, !0), document.removeEventListener("touchstart", S, !0);
    }), (h, k) => (r(), m("div", {
      ref_key: "rootRef",
      ref: v,
      class: N(["cdd-root", { "cdd-open": i.value }])
    }, [
      l("button", {
        class: "cdd-trigger",
        onClick: E,
        type: "button"
      }, [
        l("span", null, C(M.value), 1),
        k[0] || (k[0] = l("svg", {
          class: "cdd-arrow",
          viewBox: "0 0 12 7",
          width: "10",
          height: "6"
        }, [
          l("path", {
            d: "M1 1l5 5 5-5",
            stroke: "currentColor",
            "stroke-width": "1.5",
            fill: "none",
            "stroke-linecap": "round"
          })
        ], -1))
      ]),
      Q(ae, { name: "cdd-pop" }, {
        default: se(() => [
          i.value ? (r(), m("div", _e, [
            (r(!0), m(W, null, Z(t.options, (V) => (r(), m("button", {
              key: V.value,
              class: N(["cdd-opt", { "cdd-active": V.value === t.modelValue }]),
              style: g(V.value === t.modelValue ? T.value : {}),
              onClick: (w) => _(V.value),
              type: "button"
            }, C(V.label), 15, Se))), 128))
          ])) : R("", !0)
        ]),
        _: 1
      })
    ], 2));
  }
}, Lt = /* @__PURE__ */ F(we, [["__scopeId", "data-v-2eafd85a"]]), Ee = ["value", "placeholder", "disabled", "readonly"], Ce = ["type", "value", "placeholder", "disabled", "readonly"], Le = {
  __name: "Inpt_Text",
  props: {
    modelValue: { type: [String, Number], default: "" },
    type: { type: String, default: "text" },
    placeholder: { type: String, default: "" },
    color: { type: String, default: "#39c5bb" },
    plaintext: { type: Boolean, default: !1 },
    disabled: { type: Boolean, default: !1 },
    readonly: { type: Boolean, default: !1 }
  },
  emits: ["update:modelValue"],
  setup(t) {
    const p = t, e = b(() => ({
      "--inpt-color": p.color
    }));
    return ($, v) => t.plaintext ? (r(), m("textarea", {
      key: 0,
      class: "inpt-root inpt-plain",
      value: t.modelValue,
      placeholder: t.placeholder,
      disabled: t.disabled,
      readonly: t.readonly,
      style: g(e.value),
      rows: "4",
      onInput: v[0] || (v[0] = (i) => $.$emit("update:modelValue", i.target.value))
    }, null, 44, Ee)) : (r(), m("input", {
      key: 1,
      class: "inpt-root",
      type: t.type,
      value: t.modelValue,
      placeholder: t.placeholder,
      disabled: t.disabled,
      readonly: t.readonly,
      style: g(e.value),
      onInput: v[1] || (v[1] = (i) => $.$emit("update:modelValue", t.type === "number" ? i.target.value === "" ? null : Number(i.target.value) : i.target.value))
    }, null, 44, Ce));
  }
}, Ie = /* @__PURE__ */ F(Le, [["__scopeId", "data-v-87acb143"]]), Re = { class: "ccp-root" }, Te = { class: "ccp-swatch-hex" }, Xe = { class: "ccp-dialog" }, Be = { class: "ccp-footer" }, Ne = {
  __name: "Ctrl_ColorPicker",
  props: {
    modelValue: { type: String, default: "#ffffff" },
    color: { type: String, default: "#39c5bb" },
    teleport: { type: String, default: "body" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: p }) {
    oe((d) => ({
      v83e19834: e.color
    }));
    const e = t, $ = p, v = b(() => e.teleport !== "body"), i = x(!1), M = x("#ffffff"), T = x(null), E = x(null), _ = x(0), S = x(0), h = x(100), k = x(0);
    let V = !1, w = !1;
    const P = b(() => ({ background: `linear-gradient(to right, #fff, ${D(_.value, 100, 50)})` })), X = b(() => ({
      background: e.color,
      borderColor: e.color
    }));
    function D(d, n, c) {
      n /= 100, c /= 100;
      const y = n * Math.min(c, 1 - c), I = (B) => {
        const H = (B + d / 30) % 12;
        return Math.round(255 * (c - y * Math.max(Math.min(H - 3, 9 - H, 1), -1))).toString(16).padStart(2, "0");
      };
      return `#${I(0)}${I(8)}${I(4)}`;
    }
    function u(d) {
      let n = 0, c = 0, y = 0;
      d = d.replace("#", ""), d.length === 3 ? (n = parseInt(d[0] + d[0], 16) / 255, c = parseInt(d[1] + d[1], 16) / 255, y = parseInt(d[2] + d[2], 16) / 255) : d.length >= 6 && (n = parseInt(d.slice(0, 2), 16) / 255, c = parseInt(d.slice(2, 4), 16) / 255, y = parseInt(d.slice(4, 6), 16) / 255);
      const I = Math.max(n, c, y), B = Math.min(n, c, y), H = (I + B) / 2;
      let A = 0, Y = 0;
      if (I !== B) {
        const z = I - B;
        Y = H > 0.5 ? z / (2 - I - B) : z / (I + B), I === n ? A = ((c - y) / z + (c < y ? 6 : 0)) / 6 : I === c ? A = ((y - n) / z + 2) / 6 : A = ((n - c) / z + 4) / 6;
      }
      return A = Math.round(A * 360), A < 0 && (A += 360), { h: A, s: Math.round(Y * 100), l: Math.round(H * 100) };
    }
    function o(d) {
      const n = u(d);
      _.value = n.h, S.value = n.h / 360 * 100, h.value = n.s;
      const c = 100 - n.s * 0.5;
      k.value = c > 0 ? Math.max(0, Math.min(100, Math.round(100 - n.l * 100 / c))) : 0;
    }
    function a() {
      const d = h.value, n = 100 - d * 0.5, c = Math.round(n * (100 - k.value) / 100);
      M.value = D(_.value, d, c);
    }
    function s() {
      M.value = e.modelValue || "#ffffff", o(M.value), i.value = !0;
    }
    function f() {
      $("update:modelValue", M.value), i.value = !1;
    }
    function L() {
      i.value = !1;
    }
    function O(d) {
      const n = String(d ?? "");
      M.value = n, /^#[0-9a-fA-F]{6}$/.test(n.trim()) && o(n.trim());
    }
    function ue(d) {
      const n = E.value.getBoundingClientRect(), c = Math.max(0, Math.min(100, (d - n.left) / n.width * 100));
      S.value = c, _.value = Math.round(c / 100 * 360), a();
    }
    function ee(d) {
      V = !0, ue(d.clientX || d.touches[0].clientX);
    }
    function te(d, n) {
      const c = T.value.getBoundingClientRect();
      h.value = Math.max(0, Math.min(100, (d - c.left) / c.width * 100)), k.value = Math.max(0, Math.min(100, (n - c.top) / c.height * 100)), a();
    }
    function ne(d) {
      w = !0, te(d.clientX || d.touches[0].clientX, d.clientY || d.touches[0].clientY);
    }
    function K(d) {
      V && ue(d.clientX || d.touches[0].clientX), w && te(d.clientX || d.touches[0].clientX, d.clientY || d.touches[0].clientY);
    }
    function j() {
      V = !1, w = !1;
    }
    return G(() => {
      document.addEventListener("mousemove", K), document.addEventListener("mouseup", j), document.addEventListener("touchmove", K, { passive: !1 }), document.addEventListener("touchend", j);
    }), J(() => {
      document.removeEventListener("mousemove", K), document.removeEventListener("mouseup", j), document.removeEventListener("touchmove", K), document.removeEventListener("touchend", j);
    }), (d, n) => (r(), m("div", Re, [
      l("div", {
        class: "ccp-swatch",
        onClick: s
      }, [
        l("span", {
          class: "ccp-swatch-dot",
          style: g({ background: t.modelValue || "#ffffff" })
        }, null, 4),
        l("span", Te, C(t.modelValue || "#ffffff"), 1)
      ]),
      (r(), ce(ie, { to: t.teleport }, [
        Q(ae, { name: "ccp-dialog" }, {
          default: se(() => [
            i.value ? (r(), m("div", {
              key: 0,
              class: N(["ccp-overlay", { "ccp-anchored": v.value }]),
              onClick: U(L, ["self"])
            }, [
              l("div", Xe, [
                l("div", { class: "ccp-header" }, [
                  n[1] || (n[1] = l("span", null, "选取颜色", -1)),
                  l("button", {
                    class: "ccp-close",
                    onClick: L,
                    type: "button"
                  }, [...n[0] || (n[0] = [
                    l("svg", {
                      viewBox: "0 0 24 24",
                      width: "12",
                      height: "12",
                      stroke: "currentColor",
                      "stroke-width": "2.5",
                      fill: "none"
                    }, [
                      l("path", { d: "M18 6L6 18M6 6l12 12" })
                    ], -1)
                  ])])
                ]),
                l("div", {
                  class: "ccp-sv",
                  ref_key: "svRef",
                  ref: T,
                  style: g(P.value),
                  onMousedown: ne,
                  onTouchstart: U(ne, ["prevent"])
                }, [
                  l("div", {
                    class: "ccp-sv-thumb",
                    style: g({ left: h.value + "%", top: k.value + "%" })
                  }, null, 4)
                ], 36),
                l("div", {
                  class: "ccp-hue",
                  ref_key: "hueRef",
                  ref: E,
                  onMousedown: ee,
                  onTouchstart: U(ee, ["prevent"])
                }, [
                  l("div", {
                    class: "ccp-hue-thumb",
                    style: g({ left: S.value + "%" })
                  }, null, 4)
                ], 544),
                l("div", Be, [
                  l("span", {
                    class: "ccp-preview",
                    style: g({ background: M.value })
                  }, null, 4),
                  Q(Ie, {
                    "model-value": M.value,
                    color: e.color,
                    placeholder: "#ffffff",
                    style: { flex: "1", height: "28px", "text-align": "left", "font-family": "inherit" },
                    "onUpdate:modelValue": O
                  }, null, 8, ["model-value", "color"]),
                  l("button", {
                    class: "ccp-confirm",
                    style: g(X.value),
                    onClick: f,
                    type: "button",
                    title: "确定"
                  }, [...n[2] || (n[2] = [
                    l("svg", {
                      viewBox: "0 0 24 24",
                      width: "16",
                      height: "16",
                      stroke: "currentColor",
                      "stroke-width": "3",
                      fill: "none"
                    }, [
                      l("path", { d: "M20 6L9 17l-5-5" })
                    ], -1)
                  ])], 4)
                ])
              ])
            ], 2)) : R("", !0)
          ]),
          _: 1
        })
      ], 8, ["to"]))
    ]));
  }
}, It = /* @__PURE__ */ F(Ne, [["__scopeId", "data-v-15758b41"]]), De = { class: "ctl-root" }, Fe = { class: "ctl-bubble" }, Ue = { class: "ctl-labels" }, Pe = {
  __name: "Ctrl_Timeline",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 120 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "#39c5bb" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: p }) {
    oe((a) => ({
      v19473fba: e.color
    }));
    const e = t, $ = p, v = x(null), i = x(!1), M = b(() => e.max - e.min <= 60 ? 2 : 3), T = b(() => {
      const a = [], s = e.max - e.min <= 60 ? 5 : 10;
      for (let f = e.min; f <= e.max; f += s) a.push(f);
      return a;
    }), E = b(() => {
      const a = [], s = e.max - e.min <= 60 ? 15 : 30;
      for (let f = e.min; f <= e.max; f += s) a.push(f);
      return a;
    });
    function _(a) {
      const s = e.max - e.min;
      return s <= 0 ? 0 : (a - e.min) / s * 100;
    }
    function S(a) {
      const s = Math.floor(a / 60), f = a % 60;
      return `${s}:${String(f).padStart(2, "0")}`;
    }
    const h = b(() => {
      const a = e.max - e.min;
      return a <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / a * 100));
    }), k = b(() => ({
      width: h.value + "%",
      background: `linear-gradient(90deg, ${e.color}55, ${e.color})`
    })), V = b(() => ({
      left: h.value + "%"
    })), w = b(() => {
      const a = Math.max(0, Math.round(e.modelValue || 0));
      return `${Math.floor(a / 60)}:${String(a % 60).padStart(2, "0")}`;
    });
    function P(a) {
      return e.step < 1 ? parseFloat(a) : parseInt(a);
    }
    function X(a) {
      if (!v.value) return e.modelValue;
      const s = v.value.getBoundingClientRect(), f = Math.max(0, Math.min(1, (a - s.left) / s.width)), L = e.min + f * (e.max - e.min), O = Math.round(L / e.step) * e.step;
      return P(Math.max(e.min, Math.min(e.max, O)).toFixed(10));
    }
    function D(a) {
      var f, L;
      i.value = !0;
      const s = X(a.clientX || ((L = (f = a.touches) == null ? void 0 : f[0]) == null ? void 0 : L.clientX) || 0);
      s !== e.modelValue && $("update:modelValue", s);
    }
    function u(a) {
      var f, L;
      if (!i.value) return;
      const s = X(a.clientX || ((L = (f = a.touches) == null ? void 0 : f[0]) == null ? void 0 : L.clientX) || 0);
      s !== e.modelValue && $("update:modelValue", s);
    }
    function o() {
      i.value = !1;
    }
    return G(() => {
      document.addEventListener("mousemove", u), document.addEventListener("mouseup", o), document.addEventListener("touchmove", u, { passive: !1 }), document.addEventListener("touchend", o);
    }), J(() => {
      document.removeEventListener("mousemove", u), document.removeEventListener("mouseup", o), document.removeEventListener("touchmove", u), document.removeEventListener("touchend", o);
    }), (a, s) => (r(), m("div", De, [
      l("div", {
        ref_key: "trackRef",
        ref: v,
        class: N(["ctl-track", { "ctl-dragging": i.value }]),
        onMousedown: U(D, ["left"]),
        onTouchstart: U(D, ["prevent"])
      }, [
        l("div", {
          class: "ctl-fill",
          style: g(k.value)
        }, null, 4),
        (r(!0), m(W, null, Z(T.value, (f) => (r(), m("div", {
          key: "t" + f,
          class: N(["ctl-tick", { "ctl-tick-minor": f % M.value !== 0 }]),
          style: g({ left: _(f) + "%" })
        }, null, 6))), 128)),
        l("div", {
          class: "ctl-indicator",
          style: g(V.value)
        }, [
          l("span", Fe, C(w.value), 1),
          s[0] || (s[0] = l("span", { class: "ctl-thumb" }, null, -1))
        ], 4)
      ], 34),
      l("div", Ue, [
        (r(!0), m(W, null, Z(E.value, (f) => (r(), m("span", {
          key: "l" + f,
          class: "ctl-label-tick",
          style: g({ left: _(f) + "%" })
        }, C(S(f)), 5))), 128))
      ])
    ]));
  }
}, Rt = /* @__PURE__ */ F(Pe, [["__scopeId", "data-v-674af57f"]]), Ae = { class: "ui-card" }, He = { class: "ui-card-title" }, Oe = {
  key: 0,
  class: "ui-card-desc"
}, Ye = {
  __name: "UiCard",
  props: {
    title: { type: String, required: !0 },
    desc: { type: String, default: "" }
  },
  setup(t) {
    return (p, e) => (r(), m("div", Ae, [
      l("div", He, C(t.title), 1),
      t.desc ? (r(), m("div", Oe, C(t.desc), 1)) : R("", !0),
      q(p.$slots, "default", {}, void 0, !0)
    ]));
  }
}, Tt = /* @__PURE__ */ F(Ye, [["__scopeId", "data-v-b67dfbf3"]]), je = { class: "nro-overlay" }, ze = { class: "nro-badge" }, qe = {
  __name: "NotReadyOverlay",
  props: {
    text: { type: String, default: "🚧 等待实现" }
  },
  setup(t) {
    return (p, e) => (r(), m("div", je, [
      l("span", ze, C(t.text), 1)
    ]));
  }
}, Ge = /* @__PURE__ */ F(qe, [["__scopeId", "data-v-b2c4028f"]]), Je = { class: "ucr-label-col" }, Ke = {
  key: 0,
  class: "ucr-hint"
}, Qe = {
  key: 1,
  class: "ucr-slot-wrap"
}, We = {
  __name: "UiConfigRow",
  props: {
    label: { type: String, required: !0 },
    hint: { type: String, default: "" },
    stack: { type: Boolean, default: !1 },
    notReady: { type: Boolean, default: !1 },
    notReadyText: { type: String, default: "等待实现" }
  },
  setup(t) {
    return (p, e) => (r(), m("div", {
      class: N(["ucr-row", { "ucr-not-ready": t.notReady, "ucr-stack": t.stack }])
    }, [
      l("div", Je, [
        l("label", null, C(t.label), 1),
        t.hint ? (r(), m("span", Ke, C(t.hint), 1)) : R("", !0),
        t.stack ? (r(), m("div", Qe, [
          q(p.$slots, "default", {}, void 0, !0)
        ])) : R("", !0)
      ]),
      t.stack ? R("", !0) : q(p.$slots, "default", {}, void 0, !0, 0),
      t.notReady ? (r(), ce(Ge, {
        key: 1,
        text: t.notReadyText
      }, null, 8, ["text"])) : R("", !0)
    ], 2));
  }
}, Xt = /* @__PURE__ */ F(We, [["__scopeId", "data-v-4e6d2936"]]), Ze = ["checked", "disabled"], et = {
  __name: "UiSwitch",
  props: {
    modelValue: { type: Boolean, default: !1 },
    color: { type: String, default: "#39c5bb" },
    disabled: { type: Boolean, default: !1 }
  },
  emits: ["update:modelValue"],
  setup(t) {
    const p = t, e = b(() => ({
      background: p.modelValue ? p.color : void 0
    }));
    return ($, v) => (r(), m("label", {
      class: N(["uis-root", { "uis-disabled": t.disabled }])
    }, [
      l("input", {
        type: "checkbox",
        checked: t.modelValue,
        disabled: t.disabled,
        onChange: v[0] || (v[0] = (i) => $.$emit("update:modelValue", i.target.checked))
      }, null, 40, Ze),
      l("span", {
        class: "uis-track",
        style: g(e.value)
      }, null, 4)
    ], 2));
  }
}, Bt = /* @__PURE__ */ F(et, [["__scopeId", "data-v-23edf57a"]]), tt = { class: "uist-value" }, nt = 400, lt = 60, ot = {
  __name: "UiSlider",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "#39c5bb" },
    display: { type: String, default: "" },
    displayValue: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: p }) {
    const e = t, $ = p, v = x(null), i = b(() => {
      const u = e.max - e.min;
      return u <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / u * 100));
    }), M = b(() => ({
      background: `linear-gradient(to right, ${e.color}33 ${i.value}%, transparent ${i.value}%)`,
      borderColor: e.color
    }));
    function T(u) {
      return e.step < 1 ? parseFloat(u) : parseInt(u);
    }
    function E(u) {
      const o = e.modelValue + u * e.step, a = Math.max(e.min, Math.min(e.max, o)), s = T(a.toFixed(10));
      s !== e.modelValue && $("update:modelValue", s);
    }
    const _ = x(!1);
    function S(u) {
      if (!v.value) return e.modelValue;
      const o = v.value.getBoundingClientRect(), a = Math.max(0, Math.min(1, (u - o.left) / o.width)), s = e.min + a * (e.max - e.min), f = Math.round(s / e.step) * e.step;
      return T(Math.max(e.min, Math.min(e.max, f)).toFixed(10));
    }
    function h(u) {
      var f, L;
      _.value = !0;
      const o = u.target, a = o.closest(".uist-minus"), s = o.closest(".uist-plus");
      if (a)
        E(-1), P(-1);
      else if (s)
        E(1), P(1);
      else {
        const O = S(u.clientX || ((L = (f = u.touches) == null ? void 0 : f[0]) == null ? void 0 : L.clientX) || 0);
        O !== e.modelValue && $("update:modelValue", O);
      }
    }
    function k(u) {
      var a, s;
      if (!_.value) return;
      X();
      const o = S(u.clientX || ((s = (a = u.touches) == null ? void 0 : a[0]) == null ? void 0 : s.clientX) || 0);
      o !== e.modelValue && $("update:modelValue", o);
    }
    function V() {
      _.value = !1, X();
    }
    const w = x(null);
    function P(u) {
      X(), w.value = setTimeout(() => {
        w.value = setInterval(() => E(u), lt);
      }, nt);
    }
    function X() {
      w.value && (clearTimeout(w.value), clearInterval(w.value), w.value = null);
    }
    G(() => {
      document.addEventListener("mousemove", k), document.addEventListener("mouseup", V), document.addEventListener("touchmove", k, { passive: !1 }), document.addEventListener("touchend", V);
    }), J(() => {
      document.removeEventListener("mousemove", k), document.removeEventListener("mouseup", V), document.removeEventListener("touchmove", k), document.removeEventListener("touchend", V), X();
    });
    const D = b(() => {
      if (e.displayValue) return e.displayValue;
      const u = e.step < 1 ? e.modelValue.toFixed(1) : e.modelValue;
      return e.display ? `${u}${e.display}` : String(u);
    });
    return (u, o) => (r(), m("div", {
      ref_key: "rootRef",
      ref: v,
      class: N(["uist-root", { "uist-dragging": _.value }]),
      style: g(M.value),
      onMousedown: U(h, ["left"]),
      onTouchstart: U(h, ["prevent"])
    }, [
      o[0] || (o[0] = l("button", { class: "uist-btn uist-minus" }, "−", -1)),
      l("span", tt, C(D.value), 1),
      o[1] || (o[1] = l("button", { class: "uist-btn uist-plus" }, "+", -1))
    ], 38));
  }
}, Nt = /* @__PURE__ */ F(ot, [["__scopeId", "data-v-adbc4d2c"]]), at = { class: "uxt-root" }, st = {
  class: "uxt-arrow",
  viewBox: "0 0 12 6",
  width: "10",
  height: "5"
}, ut = ["fill"], ct = { class: "uxt-label" }, it = { class: "uxt-labels" }, rt = {
  __name: "UiTimeline",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 120 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "#39c5bb" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: p }) {
    const e = t, $ = p, v = x(null), i = x(!1), M = b(() => {
      const o = [], a = e.max - e.min <= 60 ? 10 : 30;
      for (let s = e.min; s <= e.max; s += a) o.push(s);
      return o;
    }), T = b(() => {
      const o = [], a = e.max - e.min <= 60 ? 15 : 30;
      for (let s = e.min; s <= e.max; s += a) o.push(s);
      return o;
    });
    function E(o) {
      const a = e.max - e.min;
      return a <= 0 ? 0 : (o - e.min) / a * 100;
    }
    function _(o) {
      const a = Math.floor(o / 60), s = o % 60;
      return `${a}:${String(s).padStart(2, "0")}`;
    }
    const S = b(() => {
      const o = e.max - e.min;
      return o <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / o * 100));
    }), h = b(() => ({
      width: S.value + "%",
      background: e.color + "33"
    })), k = b(() => ({
      left: S.value + "%"
    })), V = b(() => {
      const o = Math.max(0, Math.round(e.modelValue || 0));
      return `${Math.floor(o / 60)}:${String(o % 60).padStart(2, "0")}`;
    });
    function w(o) {
      return e.step < 1 ? parseFloat(o) : parseInt(o);
    }
    function P(o) {
      if (!v.value) return e.modelValue;
      const a = v.value.getBoundingClientRect(), s = Math.max(0, Math.min(1, (o - a.left) / a.width)), f = e.min + s * (e.max - e.min), L = Math.round(f / e.step) * e.step;
      return w(Math.max(e.min, Math.min(e.max, L)).toFixed(10));
    }
    function X(o) {
      var s, f;
      i.value = !0;
      const a = P(o.clientX || ((f = (s = o.touches) == null ? void 0 : s[0]) == null ? void 0 : f.clientX) || 0);
      a !== e.modelValue && $("update:modelValue", a);
    }
    function D(o) {
      var s, f;
      if (!i.value) return;
      const a = P(o.clientX || ((f = (s = o.touches) == null ? void 0 : s[0]) == null ? void 0 : f.clientX) || 0);
      a !== e.modelValue && $("update:modelValue", a);
    }
    function u() {
      i.value = !1;
    }
    return G(() => {
      document.addEventListener("mousemove", D), document.addEventListener("mouseup", u), document.addEventListener("touchmove", D, { passive: !1 }), document.addEventListener("touchend", u);
    }), J(() => {
      document.removeEventListener("mousemove", D), document.removeEventListener("mouseup", u), document.removeEventListener("touchmove", D), document.removeEventListener("touchend", u);
    }), (o, a) => (r(), m("div", at, [
      l("div", {
        ref_key: "trackRef",
        ref: v,
        class: "uxt-track",
        onMousedown: U(X, ["left"]),
        onTouchstart: U(X, ["prevent"])
      }, [
        l("div", {
          class: "uxt-fill",
          style: g(h.value)
        }, null, 4),
        (r(!0), m(W, null, Z(M.value, (s) => (r(), m("div", {
          key: s,
          class: "uxt-tick",
          style: g({ left: E(s) + "%" })
        }, null, 4))), 128)),
        l("div", {
          class: "uxt-indicator",
          style: g(k.value)
        }, [
          (r(), m("svg", st, [
            l("path", {
              d: "M0 0l6 6 6-6z",
              fill: e.color
            }, null, 8, ut)
          ])),
          l("span", ct, C(V.value), 1)
        ], 4)
      ], 544),
      l("div", it, [
        (r(!0), m(W, null, Z(T.value, (s) => (r(), m("span", {
          key: s,
          class: "uxt-label-tick",
          style: g({ left: E(s) + "%" })
        }, C(_(s)), 5))), 128))
      ])
    ]));
  }
}, Dt = /* @__PURE__ */ F(rt, [["__scopeId", "data-v-dff46e6f"]]), dt = {
  key: 0,
  class: "udd-drop"
}, mt = ["onClick"], ft = {
  __name: "UiDropdown",
  props: {
    modelValue: { type: [String, Number], default: "" },
    options: { type: Array, default: () => [] },
    color: { type: String, default: "#39c5bb" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: p }) {
    oe((h) => ({
      v4b105375: e.color
    }));
    const e = t, $ = p, v = x(null), i = x(!1), M = b(() => {
      const h = e.options.find((k) => k.value === e.modelValue);
      return h ? h.label : String(e.modelValue || "");
    }), T = b(() => ({
      color: e.color,
      background: e.color + "14"
    }));
    function E() {
      i.value = !i.value;
    }
    function _(h) {
      $("update:modelValue", h), i.value = !1;
    }
    function S(h) {
      v.value && !v.value.contains(h.target) && (i.value = !1);
    }
    return G(() => {
      document.addEventListener("mousedown", S, !0), document.addEventListener("touchstart", S, !0);
    }), J(() => {
      document.removeEventListener("mousedown", S, !0), document.removeEventListener("touchstart", S, !0);
    }), (h, k) => (r(), m("div", {
      ref_key: "rootRef",
      ref: v,
      class: N(["udd-root", { "udd-open": i.value }])
    }, [
      l("button", {
        class: "udd-trigger",
        onClick: E,
        type: "button"
      }, [
        l("span", null, C(M.value), 1),
        k[0] || (k[0] = l("svg", {
          class: "udd-arrow",
          viewBox: "0 0 12 7",
          width: "10",
          height: "6"
        }, [
          l("path", {
            d: "M1 1l5 5 5-5",
            stroke: "currentColor",
            "stroke-width": "1.5",
            fill: "none",
            "stroke-linecap": "round"
          })
        ], -1))
      ]),
      Q(ae, { name: "udd-pop" }, {
        default: se(() => [
          i.value ? (r(), m("div", dt, [
            (r(!0), m(W, null, Z(t.options, (V) => (r(), m("button", {
              key: V.value,
              class: N(["udd-opt", { "udd-active": V.value === t.modelValue }]),
              style: g(V.value === t.modelValue ? T.value : {}),
              onClick: (w) => _(V.value),
              type: "button"
            }, C(V.label), 15, mt))), 128))
          ])) : R("", !0)
        ]),
        _: 1
      })
    ], 2));
  }
}, Ft = /* @__PURE__ */ F(ft, [["__scopeId", "data-v-b3930c95"]]), vt = { class: "ucp-root" }, pt = { class: "ucp-swatch-hex" }, ht = { class: "ucp-dialog" }, yt = { class: "ucp-footer" }, gt = ["value"], bt = {
  __name: "UiColorPicker",
  props: {
    modelValue: { type: String, default: "#ffffff" },
    color: { type: String, default: "#39c5bb" },
    teleport: { type: String, default: "body" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: p }) {
    oe((n) => ({
      v76788648: e.color
    }));
    const e = t, $ = p, v = b(() => e.teleport !== "body"), i = x(!1), M = x("#ffffff"), T = x(null), E = x(null), _ = x(0), S = x(0), h = x(100), k = x(0);
    let V = !1, w = !1;
    const P = b(() => ({ background: `linear-gradient(to right, #fff, ${D(_.value, 100, 50)})` })), X = b(() => ({
      background: e.color,
      borderColor: e.color
    }));
    function D(n, c, y) {
      c /= 100, y /= 100;
      const I = c * Math.min(y, 1 - y), B = (H) => {
        const A = (H + n / 30) % 12;
        return Math.round(255 * (y - I * Math.max(Math.min(A - 3, 9 - A, 1), -1))).toString(16).padStart(2, "0");
      };
      return `#${B(0)}${B(8)}${B(4)}`;
    }
    function u(n) {
      let c = 0, y = 0, I = 0;
      n = n.replace("#", ""), n.length === 3 ? (c = parseInt(n[0] + n[0], 16) / 255, y = parseInt(n[1] + n[1], 16) / 255, I = parseInt(n[2] + n[2], 16) / 255) : n.length >= 6 && (c = parseInt(n.slice(0, 2), 16) / 255, y = parseInt(n.slice(2, 4), 16) / 255, I = parseInt(n.slice(4, 6), 16) / 255);
      const B = Math.max(c, y, I), H = Math.min(c, y, I), A = (B + H) / 2;
      let Y = 0, z = 0;
      if (B !== H) {
        const le = B - H;
        z = A > 0.5 ? le / (2 - B - H) : le / (B + H), B === c ? Y = ((y - I) / le + (y < I ? 6 : 0)) / 6 : B === y ? Y = ((I - c) / le + 2) / 6 : Y = ((c - y) / le + 4) / 6;
      }
      return Y = Math.round(Y * 360), Y < 0 && (Y += 360), { h: Y, s: Math.round(z * 100), l: Math.round(A * 100) };
    }
    function o(n) {
      const c = u(n);
      _.value = c.h, S.value = c.h / 360 * 100, h.value = c.s;
      const y = 100 - c.s * 0.5;
      k.value = y > 0 ? Math.max(0, Math.min(100, Math.round(100 - c.l * 100 / y))) : 0;
    }
    function a() {
      const n = h.value, c = 100 - n * 0.5, y = Math.round(c * (100 - k.value) / 100);
      M.value = D(_.value, n, y);
    }
    function s() {
      M.value = e.modelValue || "#ffffff", o(M.value), i.value = !0;
    }
    function f() {
      $("update:modelValue", M.value), i.value = !1;
    }
    function L() {
      i.value = !1;
    }
    function O(n) {
      const c = n.target.value.trim();
      M.value = c, /^#[0-9a-fA-F]{6}$/.test(c) && o(c);
    }
    function ue() {
      /^#[0-9a-fA-F]{6}$/.test(M.value) && o(M.value);
    }
    function ee(n) {
      const c = E.value.getBoundingClientRect(), y = Math.max(0, Math.min(100, (n - c.left) / c.width * 100));
      S.value = y, _.value = Math.round(y / 100 * 360), a();
    }
    function te(n) {
      V = !0, ee(n.clientX || n.touches[0].clientX);
    }
    function ne(n, c) {
      const y = T.value.getBoundingClientRect();
      h.value = Math.max(0, Math.min(100, (n - y.left) / y.width * 100)), k.value = Math.max(0, Math.min(100, (c - y.top) / y.height * 100)), a();
    }
    function K(n) {
      w = !0, ne(n.clientX || n.touches[0].clientX, n.clientY || n.touches[0].clientY);
    }
    function j(n) {
      V && ee(n.clientX || n.touches[0].clientX), w && ne(n.clientX || n.touches[0].clientX, n.clientY || n.touches[0].clientY);
    }
    function d() {
      V = !1, w = !1;
    }
    return G(() => {
      document.addEventListener("mousemove", j), document.addEventListener("mouseup", d), document.addEventListener("touchmove", j, { passive: !1 }), document.addEventListener("touchend", d);
    }), J(() => {
      document.removeEventListener("mousemove", j), document.removeEventListener("mouseup", d), document.removeEventListener("touchmove", j), document.removeEventListener("touchend", d);
    }), (n, c) => (r(), m("div", vt, [
      l("div", {
        class: "ucp-swatch",
        onClick: s
      }, [
        l("span", {
          class: "ucp-swatch-dot",
          style: g({ background: t.modelValue || "#ffffff" })
        }, null, 4),
        l("span", pt, C(t.modelValue || "#ffffff"), 1)
      ]),
      (r(), ce(ie, { to: t.teleport }, [
        Q(ae, { name: "ucp-dialog" }, {
          default: se(() => [
            i.value ? (r(), m("div", {
              key: 0,
              class: N(["ucp-overlay", { "ucp-anchored": v.value }]),
              onClick: U(L, ["self"])
            }, [
              l("div", ht, [
                l("div", { class: "ucp-header" }, [
                  c[0] || (c[0] = l("span", null, "选取颜色", -1)),
                  l("button", {
                    class: "ucp-close",
                    onClick: L
                  }, "✕")
                ]),
                l("div", {
                  class: "ucp-sv",
                  ref_key: "svRef",
                  ref: T,
                  style: g(P.value),
                  onMousedown: K,
                  onTouchstart: U(K, ["prevent"])
                }, [
                  l("div", {
                    class: "ucp-sv-thumb",
                    style: g({ left: h.value + "%", top: k.value + "%" })
                  }, null, 4)
                ], 36),
                l("div", {
                  class: "ucp-hue",
                  ref_key: "hueRef",
                  ref: E,
                  onMousedown: te,
                  onTouchstart: U(te, ["prevent"])
                }, [
                  l("div", {
                    class: "ucp-hue-thumb",
                    style: g({ left: S.value + "%" })
                  }, null, 4)
                ], 544),
                l("div", yt, [
                  l("span", {
                    class: "ucp-preview",
                    style: g({ background: M.value })
                  }, null, 4),
                  l("input", {
                    type: "text",
                    class: "ucp-hex",
                    value: M.value,
                    onInput: O,
                    onBlur: ue,
                    placeholder: "#ffffff"
                  }, null, 40, gt),
                  l("button", {
                    class: "ucp-confirm",
                    style: g(X.value),
                    onClick: f
                  }, "确定", 4)
                ])
              ])
            ], 2)) : R("", !0)
          ]),
          _: 1
        })
      ], 8, ["to"]))
    ]));
  }
}, Ut = /* @__PURE__ */ F(bt, [["__scopeId", "data-v-b631d88e"]]), kt = { class: "uid-header" }, Vt = { class: "uid-title" }, $t = { class: "uid-body" }, Mt = {
  key: 0,
  class: "uid-footer"
}, xt = {
  __name: "UiDialog",
  props: {
    show: { type: Boolean, default: !1 },
    title: { type: String, default: "" },
    color: { type: String, default: "#39c5bb" },
    teleport: { type: String, default: "body" },
    width: { type: String, default: "auto" }
  },
  emits: ["update:show"],
  setup(t) {
    const p = t, e = b(() => p.teleport !== "body"), $ = b(() => ({
      width: p.width,
      "--uid-color": p.color
    }));
    return (v, i) => (r(), ce(ie, { to: t.teleport }, [
      Q(ae, { name: "uid-pop" }, {
        default: se(() => [
          t.show ? (r(), m("div", {
            key: 0,
            class: N(["uid-overlay", { "uid-anchored": e.value }]),
            onClick: i[1] || (i[1] = U((M) => v.$emit("update:show", !1), ["self"]))
          }, [
            l("div", {
              class: "uid-box",
              style: g($.value)
            }, [
              l("div", kt, [
                l("span", Vt, C(t.title), 1),
                l("button", {
                  class: "uid-close",
                  onClick: i[0] || (i[0] = (M) => v.$emit("update:show", !1))
                }, "✕")
              ]),
              l("div", $t, [
                q(v.$slots, "default", {}, void 0, !0)
              ]),
              v.$slots.footer ? (r(), m("div", Mt, [
                q(v.$slots, "footer", {}, void 0, !0)
              ])) : R("", !0)
            ], 4)
          ], 2)) : R("", !0)
        ]),
        _: 3
      })
    ], 8, ["to"]));
  }
}, Pt = /* @__PURE__ */ F(xt, [["__scopeId", "data-v-05f7116d"]]);
export {
  St as Cntr_Card,
  wt as Cntr_Cfgrow,
  St as Ct_Card,
  It as Ctrl_ColorPicker,
  Lt as Ctrl_Dropdown,
  Ct as Ctrl_Slider,
  Et as Ctrl_Switch,
  Rt as Ctrl_Timeline,
  Ie as Inpt_Text,
  Ge as NotReadyOverlay,
  Tt as UiCard,
  Ut as UiColorPicker,
  Xt as UiConfigRow,
  Pt as UiDialog,
  Ft as UiDropdown,
  Nt as UiSlider,
  Bt as UiSwitch,
  Dt as UiTimeline
};
