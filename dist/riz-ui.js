import { openBlock as i, createElementBlock as r, createElementVNode as a, normalizeStyle as g, normalizeClass as B, createCommentVNode as _, toDisplayString as N, renderSlot as K, computed as v, createBlock as ie, Teleport as re, createVNode as ee, Transition as ae, withCtx as se, withModifiers as z, ref as E, onMounted as Q, onBeforeUnmount as Z, useCssVars as de, Fragment as te, renderList as le, createTextVNode as fe, withKeys as me, nextTick as ue } from "vue";
const j = (t, y) => {
  const e = t.__vccOpts || t;
  for (const [$, p] of y)
    e[$] = p;
  return e;
}, ve = { class: "ct-card" }, pe = { class: "ct-card-head" }, he = { class: "ct-card-head-text" }, ye = {
  key: 0,
  class: "ct-card-body"
}, ge = {
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
    return (y, e) => (i(), r("div", ve, [
      a("div", pe, [
        t.icon ? (i(), r("div", {
          key: 0,
          class: "ct-card-icon",
          style: g(t.iconInstyle)
        }, [
          a("i", {
            class: B(t.icon)
          }, null, 2)
        ], 4)) : _("", !0),
        a("div", he, [
          t.title ? (i(), r("div", {
            key: 0,
            class: "ct-card-title",
            style: g(t.titleInstyle)
          }, N(t.title), 5)) : _("", !0),
          t.desc ? (i(), r("div", {
            key: 1,
            class: "ct-card-desc",
            style: g(t.descInstyle)
          }, N(t.desc), 5)) : _("", !0)
        ])
      ]),
      y.$slots.default ? (i(), r("div", ye, [
        K(y.$slots, "default", {}, void 0, !0)
      ])) : _("", !0)
    ]));
  }
}, qt = /* @__PURE__ */ j(ge, [["__scopeId", "data-v-ca4cde43"]]), be = { class: "cfgr-head" }, ke = { class: "cfgr-head-text" }, $e = {
  key: 0,
  class: "cfgr-slot-wrap"
}, xe = {
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
    return (y, e) => (i(), r("div", {
      class: B(["cfgr-row", { "cfgr-stack": t.stack, "cfgr-disabled": t.disabled }])
    }, [
      a("div", be, [
        t.icon ? (i(), r("div", {
          key: 0,
          class: "cfgr-icon",
          style: g(t.iconInstyle)
        }, [
          a("i", {
            class: B(t.icon)
          }, null, 2)
        ], 4)) : _("", !0),
        a("div", ke, [
          t.label ? (i(), r("div", {
            key: 0,
            class: "cfgr-label",
            style: g(t.labelInstyle)
          }, N(t.label), 5)) : _("", !0),
          t.hint ? (i(), r("div", {
            key: 1,
            class: "cfgr-hint",
            style: g(t.hintInstyle)
          }, N(t.hint), 5)) : _("", !0)
        ]),
        t.stack ? _("", !0) : (i(), r("div", {
          key: 1,
          class: B(["cfgr-slot-inner", { "cfgr-slot-disabled": t.disabled }])
        }, [
          K(y.$slots, "default", {}, void 0, !0)
        ], 2))
      ]),
      t.stack ? (i(), r("div", $e, [
        a("div", {
          class: B(["cfgr-slot-inner", { "cfgr-slot-disabled": t.disabled }])
        }, [
          K(y.$slots, "default", {}, void 0, !0)
        ], 2)
      ])) : _("", !0)
    ], 2));
  }
}, Wt = /* @__PURE__ */ j(xe, [["__scopeId", "data-v-2ff8117d"]]), Ve = { class: "inbx-content" }, we = {
  __name: "Cntr_Infobox",
  props: {
    type: { type: String, default: "info" },
    text: { type: String, default: "" },
    textInstyle: { type: Object, default: () => ({}) },
    icon: { type: String, default: "" },
    iconInstyle: { type: Object, default: () => ({}) }
  },
  setup(t) {
    const y = t, e = {
      info: "fa-solid fa-circle-info",
      warn: "fa-solid fa-triangle-exclamation",
      error: "fa-solid fa-circle-exclamation"
    }, $ = v(() => e[y.type] || e.info);
    return (p, u) => (i(), r("div", {
      class: B(["inbx-root", `inbx-${t.type}`])
    }, [
      a("div", {
        class: "inbx-icon",
        style: g(t.iconInstyle)
      }, [
        a("i", {
          class: B(t.icon || $.value)
        }, null, 2)
      ], 4),
      a("div", Ve, [
        t.text ? (i(), r("div", {
          key: 0,
          class: "inbx-text",
          style: g(t.textInstyle)
        }, N(t.text), 5)) : _("", !0),
        K(p.$slots, "default", {}, void 0, !0)
      ])
    ], 2));
  }
}, Kt = /* @__PURE__ */ j(we, [["__scopeId", "data-v-7ebb1e3f"]]), Se = { class: "cdlg-header" }, Me = {
  key: 0,
  class: "cdlg-body"
}, Ce = {
  key: 1,
  class: "cdlg-footer"
}, Ie = {
  __name: "Ctnr_Dialog",
  props: {
    show: { type: Boolean, default: !1 },
    title: { type: String, default: "" },
    titleInstyle: { type: Object, default: () => ({}) },
    icon: { type: String, default: "" },
    iconInstyle: { type: Object, default: () => ({}) },
    color: { type: String, default: "" },
    teleport: { type: String, default: "body" },
    width: { type: String, default: "auto" },
    closable: { type: Boolean, default: !0 },
    maskClose: { type: Boolean, default: !0 }
  },
  emits: ["update:show"],
  setup(t, { emit: y }) {
    const e = t, $ = y, p = v(() => e.teleport !== "body"), u = v(() => e.color || "var(--rizui_themecolor)"), k = v(() => ({
      width: e.width,
      "--cdlg-color": u.value
    }));
    function w() {
      $("update:show", !1);
    }
    function L() {
      e.maskClose && w();
    }
    return (C, I) => (i(), ie(re, { to: t.teleport }, [
      ee(ae, { name: "cdlg-pop" }, {
        default: se(() => [
          t.show ? (i(), r("div", {
            key: 0,
            class: B(["cdlg-overlay", { "cdlg-anchored": p.value }]),
            onClick: z(L, ["self"])
          }, [
            a("div", {
              class: "cdlg-box",
              style: g(k.value)
            }, [
              a("div", Se, [
                t.icon ? (i(), r("div", {
                  key: 0,
                  class: "cdlg-icon",
                  style: g(t.iconInstyle)
                }, [
                  a("i", {
                    class: B(t.icon)
                  }, null, 2)
                ], 4)) : _("", !0),
                a("span", {
                  class: "cdlg-title",
                  style: g(t.titleInstyle)
                }, N(t.title), 5),
                t.closable ? (i(), r("button", {
                  key: 1,
                  class: "cdlg-close",
                  onClick: w,
                  type: "button"
                }, [...I[0] || (I[0] = [
                  a("svg", {
                    viewBox: "0 0 24 24",
                    width: "12",
                    height: "12",
                    stroke: "currentColor",
                    "stroke-width": "2.5",
                    fill: "none"
                  }, [
                    a("path", { d: "M18 6L6 18M6 6l12 12" })
                  ], -1)
                ])])) : _("", !0)
              ]),
              C.$slots.default ? (i(), r("div", Me, [
                K(C.$slots, "default", {}, void 0, !0)
              ])) : _("", !0),
              C.$slots.footer ? (i(), r("div", Ce, [
                K(C.$slots, "footer", {}, void 0, !0)
              ])) : _("", !0)
            ], 4)
          ], 2)) : _("", !0)
        ]),
        _: 3
      })
    ], 8, ["to"]));
  }
}, Gt = /* @__PURE__ */ j(Ie, [["__scopeId", "data-v-92bfaf3d"]]), Ee = ["checked", "disabled"], _e = {
  __name: "Ctrl_Switch",
  props: {
    modelValue: { type: Boolean, default: !1 },
    color: { type: String, default: "" },
    disabled: { type: Boolean, default: !1 }
  },
  emits: ["update:modelValue"],
  setup(t) {
    const y = t, e = v(() => y.color || "var(--rizui_themecolor)"), $ = v(() => ({
      background: y.modelValue ? e.value : void 0
    }));
    return (p, u) => (i(), r("label", {
      class: B(["csw-root", { "csw-disabled": t.disabled }])
    }, [
      a("input", {
        type: "checkbox",
        checked: t.modelValue,
        disabled: t.disabled,
        onChange: u[0] || (u[0] = (k) => p.$emit("update:modelValue", k.target.checked))
      }, null, 40, Ee),
      a("span", {
        class: "csw-track",
        style: g($.value)
      }, null, 4)
    ], 2));
  }
}, Jt = /* @__PURE__ */ j(_e, [["__scopeId", "data-v-4e926be9"]]), Le = { class: "csl-value" }, Te = 400, Re = 60, Be = {
  __name: "Ctrl_Slider",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "" },
    display: { type: String, default: "" },
    displayValue: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: y }) {
    const e = t, $ = y, p = E(null), u = v(() => e.color || "var(--rizui_themecolor)"), k = v(() => {
      const o = e.max - e.min;
      return o <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / o * 100));
    }), w = v(() => ({
      background: `linear-gradient(to right, color-mix(in srgb, ${u.value} 20%, transparent) ${k.value}%, transparent ${k.value}%)`,
      borderColor: u.value
    }));
    function L(o) {
      return e.step < 1 ? parseFloat(o) : parseInt(o);
    }
    function C(o) {
      const n = e.modelValue + o * e.step, c = Math.max(e.min, Math.min(e.max, n)), l = L(c.toFixed(10));
      l !== e.modelValue && $("update:modelValue", l);
    }
    const I = E(!1);
    function T(o) {
      if (!p.value) return e.modelValue;
      const n = p.value.getBoundingClientRect(), c = Math.max(0, Math.min(1, (o - n.left) / n.width)), l = e.min + c * (e.max - e.min), h = Math.round(l / e.step) * e.step;
      return L(Math.max(e.min, Math.min(e.max, h)).toFixed(10));
    }
    function x(o) {
      var h, m;
      I.value = !0;
      const n = o.target, c = n.closest(".csl-minus"), l = n.closest(".csl-plus");
      if (c)
        C(-1), D(-1);
      else if (l)
        C(1), D(1);
      else {
        const U = T(o.clientX || ((m = (h = o.touches) == null ? void 0 : h[0]) == null ? void 0 : m.clientX) || 0);
        U !== e.modelValue && $("update:modelValue", U);
      }
    }
    function M(o) {
      var c, l;
      if (!I.value) return;
      P();
      const n = T(o.clientX || ((l = (c = o.touches) == null ? void 0 : c[0]) == null ? void 0 : l.clientX) || 0);
      n !== e.modelValue && $("update:modelValue", n);
    }
    function S() {
      I.value = !1, P();
    }
    const R = E(null);
    function D(o) {
      P(), R.value = setTimeout(() => {
        R.value = setInterval(() => C(o), Re);
      }, Te);
    }
    function P() {
      R.value && (clearTimeout(R.value), clearInterval(R.value), R.value = null);
    }
    Q(() => {
      document.addEventListener("mousemove", M), document.addEventListener("mouseup", S), document.addEventListener("touchmove", M, { passive: !1 }), document.addEventListener("touchend", S);
    }), Z(() => {
      document.removeEventListener("mousemove", M), document.removeEventListener("mouseup", S), document.removeEventListener("touchmove", M), document.removeEventListener("touchend", S), P();
    });
    const b = v(() => {
      if (e.displayValue) return e.displayValue;
      const o = e.step < 1 ? e.modelValue.toFixed(1) : e.modelValue;
      return e.display ? `${o}${e.display}` : String(o);
    });
    return (o, n) => (i(), r("div", {
      ref_key: "rootRef",
      ref: p,
      class: B(["csl-root", { "csl-dragging": I.value }]),
      style: g(w.value),
      onMousedown: z(x, ["left"]),
      onTouchstart: z(x, ["prevent"])
    }, [
      n[0] || (n[0] = a("button", { class: "csl-btn csl-minus" }, "−", -1)),
      a("span", Le, N(b.value), 1),
      n[1] || (n[1] = a("button", { class: "csl-btn csl-plus" }, "+", -1))
    ], 38));
  }
}, Qt = /* @__PURE__ */ j(Be, [["__scopeId", "data-v-f29324c6"]]), Xe = {
  key: 0,
  class: "cdd-drop"
}, Ne = ["onClick"], De = {
  __name: "Ctrl_Dropdown",
  props: {
    modelValue: { type: [String, Number], default: "" },
    options: { type: Array, default: () => [] },
    color: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: y }) {
    de((x) => ({
      ec7c64f6: k.value
    }));
    const e = t, $ = y, p = E(null), u = E(!1), k = v(() => e.color || "var(--rizui_themecolor)"), w = v(() => {
      const x = e.options.find((M) => M.value === e.modelValue);
      return x ? x.label : String(e.modelValue || "");
    }), L = v(() => ({
      color: k.value,
      background: `color-mix(in srgb, ${k.value} 8%, transparent)`
    }));
    function C() {
      u.value = !u.value;
    }
    function I(x) {
      $("update:modelValue", x), u.value = !1;
    }
    function T(x) {
      p.value && !p.value.contains(x.target) && (u.value = !1);
    }
    return Q(() => {
      document.addEventListener("mousedown", T, !0), document.addEventListener("touchstart", T, !0);
    }), Z(() => {
      document.removeEventListener("mousedown", T, !0), document.removeEventListener("touchstart", T, !0);
    }), (x, M) => (i(), r("div", {
      ref_key: "rootRef",
      ref: p,
      class: B(["cdd-root", { "cdd-open": u.value }])
    }, [
      a("button", {
        class: "cdd-trigger",
        onClick: C,
        type: "button"
      }, [
        a("span", null, N(w.value), 1),
        M[0] || (M[0] = a("svg", {
          class: "cdd-arrow",
          viewBox: "0 0 12 7",
          width: "10",
          height: "6"
        }, [
          a("path", {
            d: "M1 1l5 5 5-5",
            stroke: "currentColor",
            "stroke-width": "1.5",
            fill: "none",
            "stroke-linecap": "round"
          })
        ], -1))
      ]),
      ee(ae, { name: "cdd-pop" }, {
        default: se(() => [
          u.value ? (i(), r("div", Xe, [
            (i(!0), r(te, null, le(t.options, (S) => (i(), r("button", {
              key: S.value,
              class: B(["cdd-opt", { "cdd-active": S.value === t.modelValue }]),
              style: g(S.value === t.modelValue ? L.value : {}),
              onClick: (R) => I(S.value),
              type: "button"
            }, N(S.label), 15, Ne))), 128))
          ])) : _("", !0)
        ]),
        _: 1
      })
    ], 2));
  }
}, Zt = /* @__PURE__ */ j(De, [["__scopeId", "data-v-5d296197"]]), Fe = ["value", "placeholder", "disabled", "readonly"], Ue = ["type", "value", "placeholder", "disabled", "readonly"], Oe = {
  __name: "Inpt_Text",
  props: {
    modelValue: { type: [String, Number], default: "" },
    type: { type: String, default: "text" },
    placeholder: { type: String, default: "" },
    color: { type: String, default: "" },
    plaintext: { type: Boolean, default: !1 },
    disabled: { type: Boolean, default: !1 },
    readonly: { type: Boolean, default: !1 }
  },
  emits: ["update:modelValue"],
  setup(t) {
    const y = t, e = v(() => ({
      "--inpt-color": y.color || "var(--rizui_themecolor)"
    }));
    return ($, p) => t.plaintext ? (i(), r("textarea", {
      key: 0,
      class: "inpt-root inpt-plain",
      value: t.modelValue,
      placeholder: t.placeholder,
      disabled: t.disabled,
      readonly: t.readonly,
      style: g(e.value),
      rows: "4",
      onInput: p[0] || (p[0] = (u) => $.$emit("update:modelValue", u.target.value))
    }, null, 44, Fe)) : (i(), r("input", {
      key: 1,
      class: "inpt-root",
      type: t.type,
      value: t.modelValue,
      placeholder: t.placeholder,
      disabled: t.disabled,
      readonly: t.readonly,
      style: g(e.value),
      onInput: p[1] || (p[1] = (u) => $.$emit("update:modelValue", t.type === "number" ? u.target.value === "" ? null : Number(u.target.value) : u.target.value))
    }, null, 44, Ue));
  }
}, je = /* @__PURE__ */ j(Oe, [["__scopeId", "data-v-93fbd3ac"]]), Pe = { class: "ccp-root" }, ze = { class: "ccp-swatch-hex" }, He = { class: "ccp-dialog" }, Ae = { class: "ccp-footer" }, Ye = {
  __name: "Ctrl_ColorPicker",
  props: {
    modelValue: { type: String, default: "#ffffff" },
    color: { type: String, default: "" },
    teleport: { type: String, default: "body" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: y }) {
    de((f) => ({
      ad6fc5a2: u.value
    }));
    const e = t, $ = y, p = v(() => e.teleport !== "body"), u = v(() => e.color || "var(--rizui_themecolor)"), k = E(!1), w = E("#ffffff"), L = E(null), C = E(null), I = E(0), T = E(0), x = E(100), M = E(0);
    let S = !1, R = !1;
    const D = v(() => ({ background: `linear-gradient(to right, #fff, ${b(I.value, 100, 50)})` })), P = v(() => ({
      background: u.value,
      borderColor: u.value
    }));
    function b(f, s, d) {
      s /= 100, d /= 100;
      const V = s * Math.min(d, 1 - d), O = (H) => {
        const q = (H + f / 30) % 12;
        return Math.round(255 * (d - V * Math.max(Math.min(q - 3, 9 - q, 1), -1))).toString(16).padStart(2, "0");
      };
      return `#${O(0)}${O(8)}${O(4)}`;
    }
    function o(f) {
      let s = 0, d = 0, V = 0;
      f = f.replace("#", ""), f.length === 3 ? (s = parseInt(f[0] + f[0], 16) / 255, d = parseInt(f[1] + f[1], 16) / 255, V = parseInt(f[2] + f[2], 16) / 255) : f.length >= 6 && (s = parseInt(f.slice(0, 2), 16) / 255, d = parseInt(f.slice(2, 4), 16) / 255, V = parseInt(f.slice(4, 6), 16) / 255);
      const O = Math.max(s, d, V), H = Math.min(s, d, V), q = (O + H) / 2;
      let Y = 0, W = 0;
      if (O !== H) {
        const J = O - H;
        W = q > 0.5 ? J / (2 - O - H) : J / (O + H), O === s ? Y = ((d - V) / J + (d < V ? 6 : 0)) / 6 : O === d ? Y = ((V - s) / J + 2) / 6 : Y = ((s - d) / J + 4) / 6;
      }
      return Y = Math.round(Y * 360), Y < 0 && (Y += 360), { h: Y, s: Math.round(W * 100), l: Math.round(q * 100) };
    }
    function n(f) {
      const s = o(f);
      I.value = s.h, T.value = s.h / 360 * 100, x.value = s.s;
      const d = 100 - s.s * 0.5;
      M.value = d > 0 ? Math.max(0, Math.min(100, Math.round(100 - s.l * 100 / d))) : 0;
    }
    function c() {
      const f = x.value, s = 100 - f * 0.5, d = Math.round(s * (100 - M.value) / 100);
      w.value = b(I.value, f, d);
    }
    function l() {
      w.value = e.modelValue || "#ffffff", n(w.value), k.value = !0;
    }
    function h() {
      $("update:modelValue", w.value), k.value = !1;
    }
    function m() {
      k.value = !1;
    }
    function U(f) {
      const s = String(f ?? "");
      w.value = s, /^#[0-9a-fA-F]{6}$/.test(s.trim()) && n(s.trim());
    }
    function F(f) {
      const s = C.value.getBoundingClientRect(), d = Math.max(0, Math.min(100, (f - s.left) / s.width * 100));
      T.value = d, I.value = Math.round(d / 100 * 360), c();
    }
    function G(f) {
      S = !0, F(f.clientX || f.touches[0].clientX);
    }
    function ne(f, s) {
      const d = L.value.getBoundingClientRect();
      x.value = Math.max(0, Math.min(100, (f - d.left) / d.width * 100)), M.value = Math.max(0, Math.min(100, (s - d.top) / d.height * 100)), c();
    }
    function oe(f) {
      R = !0, ne(f.clientX || f.touches[0].clientX, f.clientY || f.touches[0].clientY);
    }
    function X(f) {
      S && F(f.clientX || f.touches[0].clientX), R && ne(f.clientX || f.touches[0].clientX, f.clientY || f.touches[0].clientY);
    }
    function A() {
      S = !1, R = !1;
    }
    return Q(() => {
      document.addEventListener("mousemove", X), document.addEventListener("mouseup", A), document.addEventListener("touchmove", X, { passive: !1 }), document.addEventListener("touchend", A);
    }), Z(() => {
      document.removeEventListener("mousemove", X), document.removeEventListener("mouseup", A), document.removeEventListener("touchmove", X), document.removeEventListener("touchend", A);
    }), (f, s) => (i(), r("div", Pe, [
      a("div", {
        class: "ccp-swatch",
        onClick: l
      }, [
        a("span", {
          class: "ccp-swatch-dot",
          style: g({ background: t.modelValue || "#ffffff" })
        }, null, 4),
        a("span", ze, N(t.modelValue || "#ffffff"), 1)
      ]),
      (i(), ie(re, { to: t.teleport }, [
        ee(ae, { name: "ccp-dialog" }, {
          default: se(() => [
            k.value ? (i(), r("div", {
              key: 0,
              class: B(["ccp-overlay", { "ccp-anchored": p.value }]),
              onClick: z(m, ["self"])
            }, [
              a("div", He, [
                a("div", { class: "ccp-header" }, [
                  s[1] || (s[1] = a("span", null, "选取颜色", -1)),
                  a("button", {
                    class: "ccp-close",
                    onClick: m,
                    type: "button"
                  }, [...s[0] || (s[0] = [
                    a("svg", {
                      viewBox: "0 0 24 24",
                      width: "12",
                      height: "12",
                      stroke: "currentColor",
                      "stroke-width": "2.5",
                      fill: "none"
                    }, [
                      a("path", { d: "M18 6L6 18M6 6l12 12" })
                    ], -1)
                  ])])
                ]),
                a("div", {
                  class: "ccp-sv",
                  ref_key: "svRef",
                  ref: L,
                  style: g(D.value),
                  onMousedown: oe,
                  onTouchstart: z(oe, ["prevent"])
                }, [
                  a("div", {
                    class: "ccp-sv-thumb",
                    style: g({ left: x.value + "%", top: M.value + "%" })
                  }, null, 4)
                ], 36),
                a("div", {
                  class: "ccp-hue",
                  ref_key: "hueRef",
                  ref: C,
                  onMousedown: G,
                  onTouchstart: z(G, ["prevent"])
                }, [
                  a("div", {
                    class: "ccp-hue-thumb",
                    style: g({ left: T.value + "%" })
                  }, null, 4)
                ], 544),
                a("div", Ae, [
                  a("span", {
                    class: "ccp-preview",
                    style: g({ background: w.value })
                  }, null, 4),
                  ee(je, {
                    "model-value": w.value,
                    color: e.color,
                    placeholder: "#ffffff",
                    style: { flex: "1", height: "28px", "text-align": "left", "font-family": "inherit" },
                    "onUpdate:modelValue": U
                  }, null, 8, ["model-value", "color"]),
                  a("button", {
                    class: "ccp-confirm",
                    style: g(P.value),
                    onClick: h,
                    type: "button",
                    title: "确定"
                  }, [...s[2] || (s[2] = [
                    a("svg", {
                      viewBox: "0 0 24 24",
                      width: "16",
                      height: "16",
                      stroke: "currentColor",
                      "stroke-width": "3",
                      fill: "none"
                    }, [
                      a("path", { d: "M20 6L9 17l-5-5" })
                    ], -1)
                  ])], 4)
                ])
              ])
            ], 2)) : _("", !0)
          ]),
          _: 1
        })
      ], 8, ["to"]))
    ]));
  }
}, el = /* @__PURE__ */ j(Ye, [["__scopeId", "data-v-d77bbea9"]]), qe = { class: "ctl-bubble" }, We = {
  class: "ctl-arrow",
  viewBox: "0 0 12 6",
  width: "12",
  height: "6"
}, Ke = ["fill"], Ge = { class: "ctl-labels" }, Je = {
  __name: "Ctrl_Timeline",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 120 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: y }) {
    const e = t, $ = y, p = E(null), u = E(!1), k = v(() => e.color || "var(--rizui_themecolor)"), w = v(() => e.max - e.min <= 60 ? 2 : 3), L = v(() => {
      const l = [], h = e.max - e.min <= 60 ? 5 : 10;
      for (let m = e.min; m <= e.max; m += h) l.push(m);
      return l;
    }), C = v(() => {
      const l = [], h = e.max - e.min <= 60 ? 15 : 30;
      for (let m = e.min; m <= e.max; m += h) l.push(m);
      return l;
    });
    function I(l) {
      const h = e.max - e.min;
      return h <= 0 ? 0 : (l - e.min) / h * 100;
    }
    function T(l) {
      const h = Math.floor(l / 60), m = l % 60;
      return `${h}:${String(m).padStart(2, "0")}`;
    }
    const x = v(() => {
      const l = e.max - e.min;
      return l <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / l * 100));
    }), M = v(() => ({
      "--ctl-color": k.value
    })), S = v(() => ({
      width: x.value + "%",
      background: `color-mix(in srgb, ${k.value} 20%, transparent)`
    })), R = v(() => ({
      left: x.value + "%"
    })), D = v(() => {
      const l = Math.max(0, Math.round(e.modelValue || 0));
      return `${Math.floor(l / 60)}:${String(l % 60).padStart(2, "0")}`;
    });
    function P(l) {
      return e.step < 1 ? parseFloat(l) : parseInt(l);
    }
    function b(l) {
      if (!p.value) return e.modelValue;
      const h = p.value.getBoundingClientRect(), m = Math.max(0, Math.min(1, (l - h.left) / h.width)), U = e.min + m * (e.max - e.min), F = Math.round(U / e.step) * e.step;
      return P(Math.max(e.min, Math.min(e.max, F)).toFixed(10));
    }
    function o(l) {
      var m, U;
      u.value = !0;
      const h = b(l.clientX || ((U = (m = l.touches) == null ? void 0 : m[0]) == null ? void 0 : U.clientX) || 0);
      h !== e.modelValue && $("update:modelValue", h);
    }
    function n(l) {
      var m, U;
      if (!u.value) return;
      const h = b(l.clientX || ((U = (m = l.touches) == null ? void 0 : m[0]) == null ? void 0 : U.clientX) || 0);
      h !== e.modelValue && $("update:modelValue", h);
    }
    function c() {
      u.value = !1;
    }
    return Q(() => {
      document.addEventListener("mousemove", n), document.addEventListener("mouseup", c), document.addEventListener("touchmove", n, { passive: !1 }), document.addEventListener("touchend", c);
    }), Z(() => {
      document.removeEventListener("mousemove", n), document.removeEventListener("mouseup", c), document.removeEventListener("touchmove", n), document.removeEventListener("touchend", c);
    }), (l, h) => (i(), r("div", {
      class: "ctl-root",
      style: g(M.value)
    }, [
      a("div", {
        ref_key: "trackRef",
        ref: p,
        class: B(["ctl-track", { "ctl-dragging": u.value }]),
        onMousedown: z(o, ["left"]),
        onTouchstart: z(o, ["prevent"])
      }, [
        a("div", {
          class: "ctl-fill",
          style: g(S.value)
        }, null, 4),
        (i(!0), r(te, null, le(L.value, (m) => (i(), r("div", {
          key: "t" + m,
          class: B(["ctl-tick", { "ctl-tick-minor": m % w.value !== 0 }]),
          style: g({ left: I(m) + "%" })
        }, null, 6))), 128)),
        a("div", {
          class: "ctl-indicator",
          style: g(R.value)
        }, [
          a("span", qe, N(D.value), 1),
          (i(), r("svg", We, [
            a("path", {
              d: "M0 0l6 6 6-6z",
              fill: k.value
            }, null, 8, Ke)
          ]))
        ], 4)
      ], 34),
      a("div", Ge, [
        (i(!0), r(te, null, le(C.value, (m) => (i(), r("span", {
          key: "l" + m,
          class: "ctl-label-tick",
          style: g({ left: I(m) + "%" })
        }, N(T(m)), 5))), 128))
      ])
    ], 4));
  }
}, tl = /* @__PURE__ */ j(Je, [["__scopeId", "data-v-7d2a0003"]]), Qe = ["type", "disabled"], Ze = {
  __name: "Ctrl_Button",
  props: {
    text: { type: String, default: "" },
    textInstyle: { type: Object, default: () => ({}) },
    lIcon: { type: String, default: "" },
    lIconInstyle: { type: Object, default: () => ({}) },
    rIcon: { type: String, default: "" },
    rIconInstyle: { type: Object, default: () => ({}) },
    primary: { type: Boolean, default: !1 },
    danger: { type: Boolean, default: !1 },
    long: { type: Boolean, default: !1 },
    color: { type: String, default: "" },
    disabled: { type: Boolean, default: !1 },
    type: { type: String, default: "button" }
  },
  emits: ["click"],
  setup(t) {
    const y = t, e = v(() => ({
      "--cbtn-color": y.color || "var(--rizui_themecolor)"
    }));
    return ($, p) => (i(), r("button", {
      class: B(["cbtn-root", {
        "cbtn-primary": t.primary,
        "cbtn-danger": t.danger,
        "cbtn-long": t.long,
        "cbtn-disabled": t.disabled
      }]),
      style: g(e.value),
      type: t.type,
      disabled: t.disabled,
      onClick: p[0] || (p[0] = (u) => $.$emit("click", u))
    }, [
      t.lIcon ? (i(), r("i", {
        key: 0,
        class: B(["cbtn-icon", t.lIcon]),
        style: g(t.lIconInstyle)
      }, null, 6)) : _("", !0),
      t.text ? (i(), r("span", {
        key: 1,
        class: "cbtn-text",
        style: g(t.textInstyle)
      }, N(t.text), 5)) : _("", !0),
      t.rIcon ? (i(), r("i", {
        key: 2,
        class: B(["cbtn-icon", t.rIcon]),
        style: g(t.rIconInstyle)
      }, null, 6)) : _("", !0)
    ], 14, Qe));
  }
}, ll = /* @__PURE__ */ j(Ze, [["__scopeId", "data-v-6f0f392c"]]), et = {
  key: 0,
  class: "imd-toolbar"
}, tt = { class: "imd-toolbar-btns" }, lt = ["title", "disabled", "onClick"], nt = { class: "imd-tabs" }, ot = { class: "imd-body" }, at = ["value", "placeholder", "disabled", "onKeydown"], st = ["innerHTML"], ct = {
  __name: "Inpt_mdeditor",
  props: {
    modelValue: { type: String, default: "" },
    placeholder: { type: String, default: "在此输入 Markdown 内容…" },
    color: { type: String, default: "" },
    disabled: { type: Boolean, default: !1 },
    readonly: { type: Boolean, default: !1 },
    height: { type: String, default: "320px" },
    default: { type: String, default: "edit" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: y }) {
    const e = t, $ = y, p = E(null), u = E(e.default === "preview" ? "preview" : "edit"), k = v(() => ({
      "--imd-color": e.color || "var(--rizui_themecolor)"
    })), w = [
      { id: "bold", title: "加粗", icon: "fa-solid fa-bold" },
      { id: "italic", title: "斜体", icon: "fa-solid fa-italic" },
      { id: "strike", title: "删除线", icon: "fa-solid fa-strikethrough" },
      { id: "h2", title: "二级标题", icon: "fa-solid fa-heading" },
      { id: "quote", title: "引用", icon: "fa-solid fa-quote-left" },
      { id: "ul", title: "无序列表", icon: "fa-solid fa-list-ul" },
      { id: "ol", title: "有序列表", icon: "fa-solid fa-list-ol" },
      { id: "task", title: "任务列表", icon: "fa-solid fa-list-check" },
      { id: "code", title: "行内代码", icon: "fa-solid fa-code" },
      { id: "codeblock", title: "代码块", icon: "fa-solid fa-code-branch" },
      { id: "link", title: "链接", icon: "fa-solid fa-link" },
      { id: "image", title: "图片", icon: "fa-solid fa-image" },
      { id: "table", title: "表格", icon: "fa-solid fa-table" },
      { id: "hr", title: "分割线", icon: "fa-solid fa-minus" }
    ];
    function L(b, o = "", n = "") {
      const c = p.value;
      if (!c) return;
      const l = c.selectionStart, h = c.selectionEnd, m = e.modelValue.slice(l, h) || n, U = e.modelValue.slice(0, l) + b + m + o + e.modelValue.slice(h);
      $("update:modelValue", U), ue(() => {
        c.focus();
        const F = l + b.length + m.length;
        c.setSelectionRange(F, F);
      });
    }
    function C(b) {
      const o = p.value;
      if (!o) return;
      const n = o.selectionStart, c = e.modelValue.lastIndexOf(`
`, n - 1) + 1, l = e.modelValue.indexOf(`
`, n), h = l === -1 ? e.modelValue.length : l, m = e.modelValue.slice(c, h), U = e.modelValue.slice(0, c) + b + m + e.modelValue.slice(h);
      $("update:modelValue", U), ue(() => {
        o.focus();
        const F = c + b.length + (n - c);
        o.setSelectionRange(F, F);
      });
    }
    function I() {
      const b = p.value;
      if (!b) return;
      const o = b.selectionStart, n = b.selectionEnd, c = e.modelValue.slice(0, o) + "  " + e.modelValue.slice(n);
      $("update:modelValue", c), ue(() => {
        b.focus(), b.setSelectionRange(o + 2, o + 2);
      });
    }
    function T(b) {
      u.value !== "edit" ? (u.value = "edit", ue(() => x(b))) : x(b);
    }
    function x(b) {
      if (!e.disabled)
        switch (b) {
          case "bold":
            L("**", "**", "加粗文本");
            break;
          case "italic":
            L("*", "*", "斜体文本");
            break;
          case "strike":
            L("~~", "~~", "删除文本");
            break;
          case "h2":
            C("## ");
            break;
          case "quote":
            C("> ");
            break;
          case "ul":
            C("- ");
            break;
          case "ol":
            C("1. ");
            break;
          case "task":
            C("- [ ] ");
            break;
          case "code":
            L("`", "`", "code");
            break;
          case "codeblock":
            L("\n```\n", "\n```\n", "const a = 1");
            break;
          case "link":
            L("[", "](https://)", "链接文字");
            break;
          case "image":
            L("![", "](https://example.com/image.png)", "图片描述");
            break;
          case "table":
            L(`
| 列1 | 列2 |
| --- | --- |
| 内容 | 内容 |
`, "", "");
            break;
          case "hr":
            L(`
---
`, "", "");
            break;
        }
    }
    const M = v(() => P(e.modelValue || ""));
    function S(b) {
      return b.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;");
    }
    function R(b) {
      return /^(https?:|mailto:|#|\/|\.\/|\.\.\/|data:image\/)/i.test(b) ? b : "#";
    }
    function D(b) {
      let o = S(b);
      return o = o.replace(/`([^`]+)`/g, "<code>$1</code>"), o = o.replace(/!\[([^\]]*)\]\(([^)\s]+)\)/g, (n, c, l) => `<img src="${R(l)}" alt="${c}" loading="lazy">`), o = o.replace(/\[([^\]]+)\]\(([^)\s]+)\)/g, (n, c, l) => `<a href="${R(l)}" target="_blank" rel="noopener">${c}</a>`), o = o.replace(/\*\*([^*]+)\*\*/g, "<strong>$1</strong>"), o = o.replace(/(^|[^*])\*([^*\s][^*]*)\*/g, "$1<em>$2</em>"), o = o.replace(/~~([^~]+)~~/g, "<del>$1</del>"), o;
    }
    function P(b) {
      const n = b.replace(/\r\n/g, `
`).split(`
`), c = [];
      let l = 0, h = [];
      const m = () => {
        h.length && (c.push("<p>" + h.map(D).join("<br>") + "</p>"), h = []);
      };
      for (; l < n.length; ) {
        const F = n[l].trim();
        if (!F) {
          m(), l++;
          continue;
        }
        if (F.startsWith("```")) {
          m();
          const X = [];
          for (l++; l < n.length && !n[l].trim().startsWith("```"); )
            X.push(n[l]), l++;
          l++, c.push("<pre><code>" + S(X.join(`
`)) + "</code></pre>");
          continue;
        }
        const G = F.match(/^(#{1,6})\s+(.*)$/);
        if (G) {
          m();
          const X = G[1].length;
          c.push(`<h${X}>${D(G[2])}</h${X}>`), l++;
          continue;
        }
        if (/^(-{3,}|\*{3,}|_{3,})$/.test(F)) {
          m(), c.push("<hr>"), l++;
          continue;
        }
        if (F.includes("|") && n[l + 1] && /^\s*\|?[\s:\-|]+\|?\s*$/.test(n[l + 1].trim())) {
          m();
          const X = (d) => d.replace(/^\|/, "").replace(/\|$/, "").split("|").map((V) => V.trim()), A = X(F);
          l += 2;
          const f = [];
          for (; l < n.length && n[l].trim() && n[l].includes("|"); )
            f.push(X(n[l])), l++;
          let s = "<table><thead><tr>" + A.map((d) => "<th>" + D(d) + "</th>").join("") + "</tr></thead><tbody>";
          s += f.map((d) => "<tr>" + d.map((V) => "<td>" + D(V) + "</td>").join("") + "</tr>").join(""), s += "</tbody></table>", c.push(s);
          continue;
        }
        if (F.startsWith(">")) {
          m();
          const X = [];
          for (; l < n.length && n[l].trim().startsWith(">"); )
            X.push(n[l].trim().replace(/^>\s?/, "")), l++;
          c.push("<blockquote><p>" + X.map(D).join("<br>") + "</p></blockquote>");
          continue;
        }
        if (F.match(/^[-*+]\s+(.*)$/)) {
          for (m(), c.push("<ul>"); l < n.length; ) {
            const X = n[l].trim().match(/^[-*+]\s+(.*)$/);
            if (!X) break;
            const A = X[1].match(/^\[([ xX])\]\s+(.*)$/);
            if (A) {
              const f = A[1].toLowerCase() === "x" ? " checked" : "";
              c.push(`<li class="imd-task${f}"><input type="checkbox"${f} disabled> ${D(A[2])}</li>`);
            } else
              c.push("<li>" + D(X[1]) + "</li>");
            l++;
          }
          c.push("</ul>");
          continue;
        }
        if (F.match(/^\d+\.\s+(.*)$/)) {
          for (m(), c.push("<ol>"); l < n.length; ) {
            const X = n[l].trim().match(/^\d+\.\s+(.*)$/);
            if (!X) break;
            c.push("<li>" + D(X[1]) + "</li>"), l++;
          }
          c.push("</ol>");
          continue;
        }
        h.push(F), l++;
      }
      return m(), c.join(`
`);
    }
    return (b, o) => (i(), r("div", {
      class: B(["imd-root", { "imd-readonly": t.readonly }]),
      style: g(k.value)
    }, [
      t.readonly ? _("", !0) : (i(), r("div", et, [
        a("div", tt, [
          (i(), r(te, null, le(w, (n) => a("button", {
            key: n.id,
            class: "imd-tb",
            title: n.title,
            type: "button",
            disabled: t.disabled,
            onMousedown: o[0] || (o[0] = z(() => {
            }, ["prevent"])),
            onClick: (c) => T(n.id)
          }, [
            a("i", {
              class: B(n.icon)
            }, null, 2)
          ], 40, lt)), 64))
        ]),
        a("div", nt, [
          a("button", {
            class: B(["imd-tab", { "imd-tab-active": u.value === "edit" }]),
            type: "button",
            onMousedown: o[1] || (o[1] = z(() => {
            }, ["prevent"])),
            onClick: o[2] || (o[2] = (n) => u.value = "edit")
          }, [...o[6] || (o[6] = [
            a("i", { class: "fa-solid fa-pen" }, null, -1),
            fe(" 编辑", -1)
          ])], 34),
          a("button", {
            class: B(["imd-tab", { "imd-tab-active": u.value === "preview" }]),
            type: "button",
            onMousedown: o[3] || (o[3] = z(() => {
            }, ["prevent"])),
            onClick: o[4] || (o[4] = (n) => u.value = "preview")
          }, [...o[7] || (o[7] = [
            a("i", { class: "fa-solid fa-eye" }, null, -1),
            fe(" 预览", -1)
          ])], 34)
        ])
      ])),
      a("div", ot, [
        !t.readonly && u.value === "edit" ? (i(), r("textarea", {
          key: 0,
          ref_key: "taRef",
          ref: p,
          class: "imd-editor",
          value: t.modelValue,
          placeholder: t.placeholder,
          disabled: t.disabled,
          style: g({ height: t.height }),
          onInput: o[5] || (o[5] = (n) => b.$emit("update:modelValue", n.target.value)),
          onKeydown: me(z(I, ["prevent"]), ["tab"])
        }, null, 44, at)) : _("", !0),
        t.readonly || u.value === "preview" ? (i(), r("div", {
          key: 1,
          class: "imd-preview",
          style: g({ height: t.height })
        }, [
          a("div", {
            class: "imd-preview-inner",
            innerHTML: M.value
          }, null, 8, st)
        ], 4)) : _("", !0)
      ])
    ], 6));
  }
}, nl = /* @__PURE__ */ j(ct, [["__scopeId", "data-v-b8a33009"]]), it = { class: "ui-card" }, ut = { class: "ui-card-title" }, rt = {
  key: 0,
  class: "ui-card-desc"
}, dt = {
  __name: "UiCard",
  props: {
    title: { type: String, required: !0 },
    desc: { type: String, default: "" }
  },
  setup(t) {
    return (y, e) => (i(), r("div", it, [
      a("div", ut, N(t.title), 1),
      t.desc ? (i(), r("div", rt, N(t.desc), 1)) : _("", !0),
      K(y.$slots, "default", {}, void 0, !0)
    ]));
  }
}, ol = /* @__PURE__ */ j(dt, [["__scopeId", "data-v-b67dfbf3"]]), ft = { class: "nro-overlay" }, mt = { class: "nro-badge" }, vt = {
  __name: "NotReadyOverlay",
  props: {
    text: { type: String, default: "🚧 等待实现" }
  },
  setup(t) {
    return (y, e) => (i(), r("div", ft, [
      a("span", mt, N(t.text), 1)
    ]));
  }
}, pt = /* @__PURE__ */ j(vt, [["__scopeId", "data-v-b2c4028f"]]), ht = { class: "ucr-label-col" }, yt = {
  key: 0,
  class: "ucr-hint"
}, gt = {
  key: 1,
  class: "ucr-slot-wrap"
}, bt = {
  __name: "UiConfigRow",
  props: {
    label: { type: String, required: !0 },
    hint: { type: String, default: "" },
    stack: { type: Boolean, default: !1 },
    notReady: { type: Boolean, default: !1 },
    notReadyText: { type: String, default: "等待实现" }
  },
  setup(t) {
    return (y, e) => (i(), r("div", {
      class: B(["ucr-row", { "ucr-not-ready": t.notReady, "ucr-stack": t.stack }])
    }, [
      a("div", ht, [
        a("label", null, N(t.label), 1),
        t.hint ? (i(), r("span", yt, N(t.hint), 1)) : _("", !0),
        t.stack ? (i(), r("div", gt, [
          K(y.$slots, "default", {}, void 0, !0)
        ])) : _("", !0)
      ]),
      t.stack ? _("", !0) : K(y.$slots, "default", {}, void 0, !0, 0),
      t.notReady ? (i(), ie(pt, {
        key: 1,
        text: t.notReadyText
      }, null, 8, ["text"])) : _("", !0)
    ], 2));
  }
}, al = /* @__PURE__ */ j(bt, [["__scopeId", "data-v-4e6d2936"]]), kt = ["checked", "disabled"], $t = {
  __name: "UiSwitch",
  props: {
    modelValue: { type: Boolean, default: !1 },
    color: { type: String, default: "" },
    disabled: { type: Boolean, default: !1 }
  },
  emits: ["update:modelValue"],
  setup(t) {
    const y = t, e = v(() => y.color || "var(--rizui_themecolor)"), $ = v(() => ({
      background: y.modelValue ? e.value : void 0
    }));
    return (p, u) => (i(), r("label", {
      class: B(["uis-root", { "uis-disabled": t.disabled }])
    }, [
      a("input", {
        type: "checkbox",
        checked: t.modelValue,
        disabled: t.disabled,
        onChange: u[0] || (u[0] = (k) => p.$emit("update:modelValue", k.target.checked))
      }, null, 40, kt),
      a("span", {
        class: "uis-track",
        style: g($.value)
      }, null, 4)
    ], 2));
  }
}, sl = /* @__PURE__ */ j($t, [["__scopeId", "data-v-eb90b6df"]]), xt = { class: "uist-value" }, Vt = 400, wt = 60, St = {
  __name: "UiSlider",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "" },
    display: { type: String, default: "" },
    displayValue: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: y }) {
    const e = t, $ = y, p = E(null), u = v(() => e.color || "var(--rizui_themecolor)"), k = v(() => {
      const o = e.max - e.min;
      return o <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / o * 100));
    }), w = v(() => ({
      background: `linear-gradient(to right, color-mix(in srgb, ${u.value} 20%, transparent) ${k.value}%, transparent ${k.value}%)`,
      borderColor: u.value
    }));
    function L(o) {
      return e.step < 1 ? parseFloat(o) : parseInt(o);
    }
    function C(o) {
      const n = e.modelValue + o * e.step, c = Math.max(e.min, Math.min(e.max, n)), l = L(c.toFixed(10));
      l !== e.modelValue && $("update:modelValue", l);
    }
    const I = E(!1);
    function T(o) {
      if (!p.value) return e.modelValue;
      const n = p.value.getBoundingClientRect(), c = Math.max(0, Math.min(1, (o - n.left) / n.width)), l = e.min + c * (e.max - e.min), h = Math.round(l / e.step) * e.step;
      return L(Math.max(e.min, Math.min(e.max, h)).toFixed(10));
    }
    function x(o) {
      var h, m;
      I.value = !0;
      const n = o.target, c = n.closest(".uist-minus"), l = n.closest(".uist-plus");
      if (c)
        C(-1), D(-1);
      else if (l)
        C(1), D(1);
      else {
        const U = T(o.clientX || ((m = (h = o.touches) == null ? void 0 : h[0]) == null ? void 0 : m.clientX) || 0);
        U !== e.modelValue && $("update:modelValue", U);
      }
    }
    function M(o) {
      var c, l;
      if (!I.value) return;
      P();
      const n = T(o.clientX || ((l = (c = o.touches) == null ? void 0 : c[0]) == null ? void 0 : l.clientX) || 0);
      n !== e.modelValue && $("update:modelValue", n);
    }
    function S() {
      I.value = !1, P();
    }
    const R = E(null);
    function D(o) {
      P(), R.value = setTimeout(() => {
        R.value = setInterval(() => C(o), wt);
      }, Vt);
    }
    function P() {
      R.value && (clearTimeout(R.value), clearInterval(R.value), R.value = null);
    }
    Q(() => {
      document.addEventListener("mousemove", M), document.addEventListener("mouseup", S), document.addEventListener("touchmove", M, { passive: !1 }), document.addEventListener("touchend", S);
    }), Z(() => {
      document.removeEventListener("mousemove", M), document.removeEventListener("mouseup", S), document.removeEventListener("touchmove", M), document.removeEventListener("touchend", S), P();
    });
    const b = v(() => {
      if (e.displayValue) return e.displayValue;
      const o = e.step < 1 ? e.modelValue.toFixed(1) : e.modelValue;
      return e.display ? `${o}${e.display}` : String(o);
    });
    return (o, n) => (i(), r("div", {
      ref_key: "rootRef",
      ref: p,
      class: B(["uist-root", { "uist-dragging": I.value }]),
      style: g(w.value),
      onMousedown: z(x, ["left"]),
      onTouchstart: z(x, ["prevent"])
    }, [
      n[0] || (n[0] = a("button", { class: "uist-btn uist-minus" }, "−", -1)),
      a("span", xt, N(b.value), 1),
      n[1] || (n[1] = a("button", { class: "uist-btn uist-plus" }, "+", -1))
    ], 38));
  }
}, cl = /* @__PURE__ */ j(St, [["__scopeId", "data-v-0ec10baa"]]), Mt = { class: "uxt-root" }, Ct = {
  class: "uxt-arrow",
  viewBox: "0 0 12 6",
  width: "10",
  height: "5"
}, It = ["fill"], Et = { class: "uxt-label" }, _t = { class: "uxt-labels" }, Lt = {
  __name: "UiTimeline",
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 120 },
    step: { type: Number, default: 1 },
    color: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: y }) {
    const e = t, $ = y, p = E(null), u = E(!1), k = v(() => e.color || "var(--rizui_themecolor)"), w = v(() => {
      const n = [], c = e.max - e.min <= 60 ? 10 : 30;
      for (let l = e.min; l <= e.max; l += c) n.push(l);
      return n;
    }), L = v(() => {
      const n = [], c = e.max - e.min <= 60 ? 15 : 30;
      for (let l = e.min; l <= e.max; l += c) n.push(l);
      return n;
    });
    function C(n) {
      const c = e.max - e.min;
      return c <= 0 ? 0 : (n - e.min) / c * 100;
    }
    function I(n) {
      const c = Math.floor(n / 60), l = n % 60;
      return `${c}:${String(l).padStart(2, "0")}`;
    }
    const T = v(() => {
      const n = e.max - e.min;
      return n <= 0 ? 0 : Math.max(0, Math.min(100, (e.modelValue - e.min) / n * 100));
    }), x = v(() => ({
      width: T.value + "%",
      background: `color-mix(in srgb, ${k.value} 20%, transparent)`
    })), M = v(() => ({
      left: T.value + "%"
    })), S = v(() => {
      const n = Math.max(0, Math.round(e.modelValue || 0));
      return `${Math.floor(n / 60)}:${String(n % 60).padStart(2, "0")}`;
    });
    function R(n) {
      return e.step < 1 ? parseFloat(n) : parseInt(n);
    }
    function D(n) {
      if (!p.value) return e.modelValue;
      const c = p.value.getBoundingClientRect(), l = Math.max(0, Math.min(1, (n - c.left) / c.width)), h = e.min + l * (e.max - e.min), m = Math.round(h / e.step) * e.step;
      return R(Math.max(e.min, Math.min(e.max, m)).toFixed(10));
    }
    function P(n) {
      var l, h;
      u.value = !0;
      const c = D(n.clientX || ((h = (l = n.touches) == null ? void 0 : l[0]) == null ? void 0 : h.clientX) || 0);
      c !== e.modelValue && $("update:modelValue", c);
    }
    function b(n) {
      var l, h;
      if (!u.value) return;
      const c = D(n.clientX || ((h = (l = n.touches) == null ? void 0 : l[0]) == null ? void 0 : h.clientX) || 0);
      c !== e.modelValue && $("update:modelValue", c);
    }
    function o() {
      u.value = !1;
    }
    return Q(() => {
      document.addEventListener("mousemove", b), document.addEventListener("mouseup", o), document.addEventListener("touchmove", b, { passive: !1 }), document.addEventListener("touchend", o);
    }), Z(() => {
      document.removeEventListener("mousemove", b), document.removeEventListener("mouseup", o), document.removeEventListener("touchmove", b), document.removeEventListener("touchend", o);
    }), (n, c) => (i(), r("div", Mt, [
      a("div", {
        ref_key: "trackRef",
        ref: p,
        class: "uxt-track",
        onMousedown: z(P, ["left"]),
        onTouchstart: z(P, ["prevent"])
      }, [
        a("div", {
          class: "uxt-fill",
          style: g(x.value)
        }, null, 4),
        (i(!0), r(te, null, le(w.value, (l) => (i(), r("div", {
          key: l,
          class: "uxt-tick",
          style: g({ left: C(l) + "%" })
        }, null, 4))), 128)),
        a("div", {
          class: "uxt-indicator",
          style: g(M.value)
        }, [
          (i(), r("svg", Ct, [
            a("path", {
              d: "M0 0l6 6 6-6z",
              fill: k.value
            }, null, 8, It)
          ])),
          a("span", Et, N(S.value), 1)
        ], 4)
      ], 544),
      a("div", _t, [
        (i(!0), r(te, null, le(L.value, (l) => (i(), r("span", {
          key: l,
          class: "uxt-label-tick",
          style: g({ left: C(l) + "%" })
        }, N(I(l)), 5))), 128))
      ])
    ]));
  }
}, il = /* @__PURE__ */ j(Lt, [["__scopeId", "data-v-fc0c2f4a"]]), Tt = {
  key: 0,
  class: "udd-drop"
}, Rt = ["onClick"], Bt = {
  __name: "UiDropdown",
  props: {
    modelValue: { type: [String, Number], default: "" },
    options: { type: Array, default: () => [] },
    color: { type: String, default: "" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: y }) {
    de((x) => ({
      b64c2a56: k.value
    }));
    const e = t, $ = y, p = E(null), u = E(!1), k = v(() => e.color || "var(--rizui_themecolor)"), w = v(() => {
      const x = e.options.find((M) => M.value === e.modelValue);
      return x ? x.label : String(e.modelValue || "");
    }), L = v(() => ({
      color: k.value,
      background: `color-mix(in srgb, ${k.value} 8%, transparent)`
    }));
    function C() {
      u.value = !u.value;
    }
    function I(x) {
      $("update:modelValue", x), u.value = !1;
    }
    function T(x) {
      p.value && !p.value.contains(x.target) && (u.value = !1);
    }
    return Q(() => {
      document.addEventListener("mousedown", T, !0), document.addEventListener("touchstart", T, !0);
    }), Z(() => {
      document.removeEventListener("mousedown", T, !0), document.removeEventListener("touchstart", T, !0);
    }), (x, M) => (i(), r("div", {
      ref_key: "rootRef",
      ref: p,
      class: B(["udd-root", { "udd-open": u.value }])
    }, [
      a("button", {
        class: "udd-trigger",
        onClick: C,
        type: "button"
      }, [
        a("span", null, N(w.value), 1),
        M[0] || (M[0] = a("svg", {
          class: "udd-arrow",
          viewBox: "0 0 12 7",
          width: "10",
          height: "6"
        }, [
          a("path", {
            d: "M1 1l5 5 5-5",
            stroke: "currentColor",
            "stroke-width": "1.5",
            fill: "none",
            "stroke-linecap": "round"
          })
        ], -1))
      ]),
      ee(ae, { name: "udd-pop" }, {
        default: se(() => [
          u.value ? (i(), r("div", Tt, [
            (i(!0), r(te, null, le(t.options, (S) => (i(), r("button", {
              key: S.value,
              class: B(["udd-opt", { "udd-active": S.value === t.modelValue }]),
              style: g(S.value === t.modelValue ? L.value : {}),
              onClick: (R) => I(S.value),
              type: "button"
            }, N(S.label), 15, Rt))), 128))
          ])) : _("", !0)
        ]),
        _: 1
      })
    ], 2));
  }
}, ul = /* @__PURE__ */ j(Bt, [["__scopeId", "data-v-2f2af13d"]]), Xt = { class: "ucp-root" }, Nt = { class: "ucp-swatch-hex" }, Dt = { class: "ucp-dialog" }, Ft = { class: "ucp-footer" }, Ut = ["value"], Ot = {
  __name: "UiColorPicker",
  props: {
    modelValue: { type: String, default: "#ffffff" },
    color: { type: String, default: "" },
    teleport: { type: String, default: "body" }
  },
  emits: ["update:modelValue"],
  setup(t, { emit: y }) {
    de((s) => ({
      v7fbf371c: u.value
    }));
    const e = t, $ = y, p = v(() => e.teleport !== "body"), u = v(() => e.color || "var(--rizui_themecolor)"), k = E(!1), w = E("#ffffff"), L = E(null), C = E(null), I = E(0), T = E(0), x = E(100), M = E(0);
    let S = !1, R = !1;
    const D = v(() => ({ background: `linear-gradient(to right, #fff, ${b(I.value, 100, 50)})` })), P = v(() => ({
      background: u.value,
      borderColor: u.value
    }));
    function b(s, d, V) {
      d /= 100, V /= 100;
      const O = d * Math.min(V, 1 - V), H = (q) => {
        const Y = (q + s / 30) % 12;
        return Math.round(255 * (V - O * Math.max(Math.min(Y - 3, 9 - Y, 1), -1))).toString(16).padStart(2, "0");
      };
      return `#${H(0)}${H(8)}${H(4)}`;
    }
    function o(s) {
      let d = 0, V = 0, O = 0;
      s = s.replace("#", ""), s.length === 3 ? (d = parseInt(s[0] + s[0], 16) / 255, V = parseInt(s[1] + s[1], 16) / 255, O = parseInt(s[2] + s[2], 16) / 255) : s.length >= 6 && (d = parseInt(s.slice(0, 2), 16) / 255, V = parseInt(s.slice(2, 4), 16) / 255, O = parseInt(s.slice(4, 6), 16) / 255);
      const H = Math.max(d, V, O), q = Math.min(d, V, O), Y = (H + q) / 2;
      let W = 0, J = 0;
      if (H !== q) {
        const ce = H - q;
        J = Y > 0.5 ? ce / (2 - H - q) : ce / (H + q), H === d ? W = ((V - O) / ce + (V < O ? 6 : 0)) / 6 : H === V ? W = ((O - d) / ce + 2) / 6 : W = ((d - V) / ce + 4) / 6;
      }
      return W = Math.round(W * 360), W < 0 && (W += 360), { h: W, s: Math.round(J * 100), l: Math.round(Y * 100) };
    }
    function n(s) {
      const d = o(s);
      I.value = d.h, T.value = d.h / 360 * 100, x.value = d.s;
      const V = 100 - d.s * 0.5;
      M.value = V > 0 ? Math.max(0, Math.min(100, Math.round(100 - d.l * 100 / V))) : 0;
    }
    function c() {
      const s = x.value, d = 100 - s * 0.5, V = Math.round(d * (100 - M.value) / 100);
      w.value = b(I.value, s, V);
    }
    function l() {
      w.value = e.modelValue || "#ffffff", n(w.value), k.value = !0;
    }
    function h() {
      $("update:modelValue", w.value), k.value = !1;
    }
    function m() {
      k.value = !1;
    }
    function U(s) {
      const d = s.target.value.trim();
      w.value = d, /^#[0-9a-fA-F]{6}$/.test(d) && n(d);
    }
    function F() {
      /^#[0-9a-fA-F]{6}$/.test(w.value) && n(w.value);
    }
    function G(s) {
      const d = C.value.getBoundingClientRect(), V = Math.max(0, Math.min(100, (s - d.left) / d.width * 100));
      T.value = V, I.value = Math.round(V / 100 * 360), c();
    }
    function ne(s) {
      S = !0, G(s.clientX || s.touches[0].clientX);
    }
    function oe(s, d) {
      const V = L.value.getBoundingClientRect();
      x.value = Math.max(0, Math.min(100, (s - V.left) / V.width * 100)), M.value = Math.max(0, Math.min(100, (d - V.top) / V.height * 100)), c();
    }
    function X(s) {
      R = !0, oe(s.clientX || s.touches[0].clientX, s.clientY || s.touches[0].clientY);
    }
    function A(s) {
      S && G(s.clientX || s.touches[0].clientX), R && oe(s.clientX || s.touches[0].clientX, s.clientY || s.touches[0].clientY);
    }
    function f() {
      S = !1, R = !1;
    }
    return Q(() => {
      document.addEventListener("mousemove", A), document.addEventListener("mouseup", f), document.addEventListener("touchmove", A, { passive: !1 }), document.addEventListener("touchend", f);
    }), Z(() => {
      document.removeEventListener("mousemove", A), document.removeEventListener("mouseup", f), document.removeEventListener("touchmove", A), document.removeEventListener("touchend", f);
    }), (s, d) => (i(), r("div", Xt, [
      a("div", {
        class: "ucp-swatch",
        onClick: l
      }, [
        a("span", {
          class: "ucp-swatch-dot",
          style: g({ background: t.modelValue || "#ffffff" })
        }, null, 4),
        a("span", Nt, N(t.modelValue || "#ffffff"), 1)
      ]),
      (i(), ie(re, { to: t.teleport }, [
        ee(ae, { name: "ucp-dialog" }, {
          default: se(() => [
            k.value ? (i(), r("div", {
              key: 0,
              class: B(["ucp-overlay", { "ucp-anchored": p.value }]),
              onClick: z(m, ["self"])
            }, [
              a("div", Dt, [
                a("div", { class: "ucp-header" }, [
                  d[0] || (d[0] = a("span", null, "选取颜色", -1)),
                  a("button", {
                    class: "ucp-close",
                    onClick: m
                  }, "✕")
                ]),
                a("div", {
                  class: "ucp-sv",
                  ref_key: "svRef",
                  ref: L,
                  style: g(D.value),
                  onMousedown: X,
                  onTouchstart: z(X, ["prevent"])
                }, [
                  a("div", {
                    class: "ucp-sv-thumb",
                    style: g({ left: x.value + "%", top: M.value + "%" })
                  }, null, 4)
                ], 36),
                a("div", {
                  class: "ucp-hue",
                  ref_key: "hueRef",
                  ref: C,
                  onMousedown: ne,
                  onTouchstart: z(ne, ["prevent"])
                }, [
                  a("div", {
                    class: "ucp-hue-thumb",
                    style: g({ left: T.value + "%" })
                  }, null, 4)
                ], 544),
                a("div", Ft, [
                  a("span", {
                    class: "ucp-preview",
                    style: g({ background: w.value })
                  }, null, 4),
                  a("input", {
                    type: "text",
                    class: "ucp-hex",
                    value: w.value,
                    onInput: U,
                    onBlur: F,
                    placeholder: "#ffffff"
                  }, null, 40, Ut),
                  a("button", {
                    class: "ucp-confirm",
                    style: g(P.value),
                    onClick: h
                  }, "确定", 4)
                ])
              ])
            ], 2)) : _("", !0)
          ]),
          _: 1
        })
      ], 8, ["to"]))
    ]));
  }
}, rl = /* @__PURE__ */ j(Ot, [["__scopeId", "data-v-d055fd73"]]), jt = { class: "uid-header" }, Pt = { class: "uid-title" }, zt = { class: "uid-body" }, Ht = {
  key: 0,
  class: "uid-footer"
}, At = {
  __name: "UiDialog",
  props: {
    show: { type: Boolean, default: !1 },
    title: { type: String, default: "" },
    color: { type: String, default: "" },
    teleport: { type: String, default: "body" },
    width: { type: String, default: "auto" }
  },
  emits: ["update:show"],
  setup(t) {
    const y = t, e = v(() => y.teleport !== "body"), $ = v(() => y.color || "var(--rizui_themecolor)"), p = v(() => ({
      width: y.width,
      "--uid-color": $.value
    }));
    return (u, k) => (i(), ie(re, { to: t.teleport }, [
      ee(ae, { name: "uid-pop" }, {
        default: se(() => [
          t.show ? (i(), r("div", {
            key: 0,
            class: B(["uid-overlay", { "uid-anchored": e.value }]),
            onClick: k[1] || (k[1] = z((w) => u.$emit("update:show", !1), ["self"]))
          }, [
            a("div", {
              class: "uid-box",
              style: g(p.value)
            }, [
              a("div", jt, [
                a("span", Pt, N(t.title), 1),
                a("button", {
                  class: "uid-close",
                  onClick: k[0] || (k[0] = (w) => u.$emit("update:show", !1))
                }, "✕")
              ]),
              a("div", zt, [
                K(u.$slots, "default", {}, void 0, !0)
              ]),
              u.$slots.footer ? (i(), r("div", Ht, [
                K(u.$slots, "footer", {}, void 0, !0)
              ])) : _("", !0)
            ], 4)
          ], 2)) : _("", !0)
        ]),
        _: 3
      })
    ], 8, ["to"]));
  }
}, dl = /* @__PURE__ */ j(At, [["__scopeId", "data-v-9d712284"]]);
export {
  qt as Cntr_Card,
  Wt as Cntr_Cfgrow,
  Kt as Cntr_Infobox,
  qt as Ct_Card,
  Gt as Ctnr_Dialog,
  ll as Ctrl_Button,
  el as Ctrl_ColorPicker,
  Zt as Ctrl_Dropdown,
  Qt as Ctrl_Slider,
  Jt as Ctrl_Switch,
  tl as Ctrl_Timeline,
  je as Inpt_Text,
  nl as Inpt_mdeditor,
  pt as NotReadyOverlay,
  ol as UiCard,
  rl as UiColorPicker,
  al as UiConfigRow,
  dl as UiDialog,
  ul as UiDropdown,
  cl as UiSlider,
  sl as UiSwitch,
  il as UiTimeline
};
