(() => {
  const root = document.documentElement;
  const savedTheme = localStorage.getItem('webkit-theme');
  if (savedTheme) root.dataset.theme = savedTheme;
  const savedPreset = localStorage.getItem('webkit-theme-preset');
  if (savedPreset) root.dataset.themePreset = savedPreset;

  document.addEventListener('click', (event) => {
    const themeButton = event.target.closest('[data-theme-value]');
    if (themeButton) {
      const value = themeButton.dataset.themeValue;
      if (value === 'system') {
        delete root.dataset.theme;
        localStorage.removeItem('webkit-theme');
      } else {
        root.dataset.theme = value;
        localStorage.setItem('webkit-theme', value);
      }
    }

    const presetButton = event.target.closest('[data-theme-preset]');
    if (presetButton) {
      const value = presetButton.dataset.themePreset;
      if (value) {
        root.dataset.themePreset = value;
        localStorage.setItem('webkit-theme-preset', value);
      }
    }

    const openButton = event.target.closest('[data-dialog-open]');
    if (openButton) document.getElementById(openButton.dataset.dialogOpen)?.showModal();

    const closeButton = event.target.closest('[data-dialog-close]');
    if (closeButton) document.getElementById(closeButton.dataset.dialogClose)?.close();
  });
})();
